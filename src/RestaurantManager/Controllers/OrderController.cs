using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantManager.Utilities;
using Stripe.Checkout;
using Microsoft.AspNetCore.Http.Extensions;
using RestaurantManager.Enums;
using RestaurantManager.Services;

namespace RestaurantManager.Controllers;

[Authorize]
public class OrderController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // Index Action to load menu items and optionally view the cart
    public async Task<IActionResult> Index(OrderType? selectedType, bool viewCart = false, string tag = "all", int? reservationId = null)
    {
        int? userId = GetUserId();
        if (userId == null)
            return RedirectToAction("Login");  // Redirect to login if no user is found 

        selectedType ??= OrderType.TakeOut;

        selectedType = reservationId == null ? selectedType : OrderType.DineIn;

        List<MenuItem> menuItems = await GetMenuItemsWithTags(tag);
        List<DietaryTag> dietaryTags = [.. _context.DietaryTags];
        Order? cart = GetOrCreateCartOrder(userId.Value, selectedType.Value);

        ViewBag.DietaryTags = dietaryTags;
        ViewBag.OrderMenuItems = cart?.OrderMenuItems;
        ViewBag.Cart = cart;
        ViewBag.ViewCart = viewCart;
        ViewBag.SelectedType = selectedType;
        ViewBag.ReservationId = reservationId;

        return View(menuItems);
    }

    // Get the user ID from claims
    private int? GetUserId()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

        Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
    }

    private static async Task<double?> CalculateDistance(string deliveryLocation)
    {
        HttpClient httpClient = new();

        DistanceService distanceService = new(httpClient);

        // We are using the Remedy Cafe as the location of Havest & Hearth.
        double? distanceInKM = await distanceService.GetDrivingDistanceAsync("8631 109 St NW, Edmonton, AB T6G 1E8", deliveryLocation);

        return distanceInKM;
    }

    // Get menu items with dietary tags
    private async Task<List<MenuItem>> GetMenuItemsWithTags(string tag = "all")
    {
        IQueryable<MenuItem> query = _context.MenuItems
            .Where(m => m.IsAvailable)
            .Include(m => m.MenuItemDietaryTags!)
                .ThenInclude(md => md.DietaryTag);

        if (tag != "all")
        {
            query = query.Where(m =>
                m.MenuItemDietaryTags != null &&
                m.MenuItemDietaryTags.Any(md =>
                    md.DietaryTag != null &&
                    md.DietaryTag.Name.Equals(tag)));
        }

        return await query.ToListAsync();
    }

    // Save the selected order type
    [HttpPost]
    public IActionResult SetOrderType(OrderType selectedType)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        var cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);

        // Include the selectedType as a query parameter in the redirect
        return RedirectToAction("Index", new { selectedType = selectedType });
    }

    // Action to view cart in a partial
    public IActionResult Cart(OrderType selectedType)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        Order? cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);

        if (cartOrder == null)
        {
            return RedirectToAction("Index", "Order"); // Show error or redirect if cart is empty
        }

        return PartialView("_CartPartial", cartOrder);
    }

    // Checkout process to save order
    [HttpGet]
    public async Task<IActionResult> Checkout(OrderType selectedType, int addressId, int? reservationId)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        Order? cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);
        if (cartOrder == null || cartOrder.OrderMenuItems == null || cartOrder.OrderMenuItems.Count == 0)
        {
            return RedirectToAction("Index", "Order"); // Show error or redirect if cart is empty
        }

        if (reservationId.HasValue)
        { cartOrder.ReservationId = reservationId.Value; }

        UserAddress? userAddress = addressId == 0 ? cartOrder.User.UserAddresses?.FirstOrDefault() : cartOrder.User.UserAddresses?.FirstOrDefault(ua => ua.Id == addressId);

        cartOrder.Subtotal = CalculateSubtotal([.. cartOrder.OrderMenuItems!.Select(i => (i.Quantity, i.MenuItem.Price))]);

        // Get distance, calculate subtotal, calculate delivery fee.
        if (selectedType == OrderType.Delivery && userAddress != null)
        {
            try
            {
                double? deliveryDistance = await CalculateDistance(userAddress.AddressLine1);

                if (deliveryDistance.HasValue)
                {
                    deliveryDistance = Math.Round(deliveryDistance.Value, 2);
                    cartOrder.DeliveryFee = CalculateDeliveryFee(cartOrder.Subtotal, deliveryDistance.Value, selectedType) ?? 0;
                    ViewBag.DeliveryDistance = deliveryDistance;
                }
                else
                    throw new Exception("Could not calculate delivery distance.");
            }
            catch (Exception ex)
            {
                ViewBag.DeliveryError = ex.Message;
            }
        }

        cartOrder.Tax = CalculateTax(cartOrder.Subtotal, cartOrder.DeliveryFee ?? 0);
        ViewBag.CartOrder = cartOrder;

        return View(cartOrder); // Redirect to confirmation page
    }

    [HttpPost]
    public async Task<IActionResult> SubmitCheckout(
        OrderType selectedType,
        decimal? customTipAmount,
        string tipAmount,
        DateTime? scheduledTime,
        int? selectedAddressId,
        string? deliveryInstructions,
        double deliveryDistanceKm,
        int? reservationId,
        bool redeemPoints = false)
    {
        // Get User 
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId.Value);
        if (user == null) return RedirectToAction("Error");

        // Retrieve cart from session
        var cartOrder = HttpContext.Session.GetObject<Order>($"cart_order_{userId}");

        if (cartOrder == null || cartOrder.OrderMenuItems == null || cartOrder.OrderMenuItems.Count == 0)
        {
            return RedirectToAction("Index", "Order");
        }

        // Set reservation ID
        if (reservationId.HasValue)
        {
            cartOrder.ReservationId = reservationId.Value;
        }

        // Set order type
        cartOrder.Type = selectedType;

        cartOrder.DeliveryInstructions = deliveryInstructions;

        // Order Date
        cartOrder.OrderDate = DateTime.Now;

        if (selectedType != OrderType.DineIn)
            cartOrder.ScheduledTime = scheduledTime ?? DateTime.Now.AddMinutes(30);

        // Calculate Subtotal 
        cartOrder.Subtotal = CalculateSubtotal([.. cartOrder.OrderMenuItems!.Select(i => (i.Quantity, i.MenuItem.Price))]);

        // Calculate Delivery Fee
        cartOrder.DeliveryFee = CalculateDeliveryFee(cartOrder.Subtotal, deliveryDistanceKm, selectedType);

        // Calculate Tax 
        cartOrder.Tax = CalculateTax(cartOrder.Subtotal, cartOrder.DeliveryFee);

        // Calculate Tip 
        cartOrder.TipAmount = CalculateTip(tipAmount, customTipAmount);

        // Calculate Rewards 
        decimal rewardsDiscount = CalculateRewardDiscount(redeemPoints, user.RewardsPoints, out int pointsUsed);

        // Calculate Total
        List<decimal> totalTally = [
            cartOrder.Subtotal,
            cartOrder.TipAmount,
            cartOrder.Tax,
            cartOrder.DeliveryFee ?? 0,
            rewardsDiscount
        ];

        decimal total = totalTally.Sum();
        cartOrder.Total = total > 0 ? total : 0;

        cartOrder.AddressId = selectedAddressId;

        UserAddress? selectedAddress = _context.UserAddresses.FirstOrDefault(a => a.Id == selectedAddressId && a.UserId == userId);

        // // Remove cart from session
        HttpContext.Session.Remove($"cart_order_{userId}");

        // // Add cart data to session
        HttpContext.Session.SetObject($"full_cart_order", cartOrder);
        HttpContext.Session.SetInt32($"user_id", user.Id);
        HttpContext.Session.SetInt32($"points_used", pointsUsed);

        TempData["RewardsDiscount"] = rewardsDiscount.ToString();

        // Process payment with stripe checkout
        string domain = HttpContext.Request.GetEncodedUrl();
        string rootdomain = domain.Remove(domain.Length - 21);
        var options = new SessionCreateOptions
        {
            // SuccessUrl = rootdomain + $"/Order/Receipt?orderId={cartOrder.Id}",
            SuccessUrl = rootdomain + $"/Order/Confirm",
            CancelUrl = rootdomain + $"/Order",
            LineItems = new List<SessionLineItemOptions>
      {
        new SessionLineItemOptions
        {

          PriceData = new SessionLineItemPriceDataOptions
          {
            UnitAmount = (long)(total * 100), // Convert to cents
            Currency = "cad",
            ProductData = new SessionLineItemPriceDataProductDataOptions
            {
              Name = "Order Total",
              Description = "Thank you for your business!"
            }
          },
          Quantity = 1
        }
      },
            Mode = "payment",
            CustomerEmail = User.Identity?.Name,
        };

        var service = new SessionService();
        Session session = service.Create(options);
        Response.Headers.Append("Location", session.Url);
        TempData["Session"] = session.Id;
        return new StatusCodeResult(303);
    }

    public async Task<IActionResult> Confirm()
    {
        var service = new SessionService();
        Session session = service.Get(TempData["Session"].ToString());

        // Retrieve cart from session
        var cartOrder = HttpContext.Session.GetObject<Order>($"full_cart_order");
        int userId = HttpContext.Session.GetInt32($"user_id") ?? 0;
        int pointsUsed = HttpContext.Session.GetInt32($"points_used") ?? 0;

        // // Remove cart from session
        HttpContext.Session.Remove($"full_cart_order");
        HttpContext.Session.Remove($"user_id");
        HttpContext.Session.Remove($"points_used");

        if (session.PaymentStatus == "paid")
        {
            // Save the order and update user points
            await SaveOrderAndUserPoints(cartOrder, userId, pointsUsed);

            return RedirectToAction("Receipt", new { orderId = cartOrder.Id, reservationId = cartOrder.ReservationId });
        }
        else
        {
            return RedirectToAction("Index", "Order");
        }
    }

    // Method to get or create a cart order for the user
    private Order? GetOrCreateCartOrder(int userId, OrderType selectedType)
    {
        Order? cartOrder = HttpContext.Session.GetObject<Order>($"cart_order_{userId}");

        if (cartOrder == null)
        {
            User? user = _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UserAddresses)
                .FirstOrDefault();

            if (user == null) return null;

            cartOrder = new Order
            {
                Status = OrderStatus.InProgress,
                UserId = userId,
                User = user,
                Type = selectedType,
                Subtotal = 0,
                Tax = 0,
                TipAmount = 0,
                Total = 0,
                OrderDate = DateTime.Now,
                OrderMenuItems = [],
            };
        }

        cartOrder.Type = selectedType;
        HttpContext.Session.SetObject($"cart_order_{userId}", cartOrder);
        return cartOrder;
    }

    // Add menu item to the cart
    public IActionResult AddToCart(OrderType selectedType, int menuItemId, int? reservationId)
    {
        int? userId = GetUserId();
        if (!userId.HasValue) return RedirectToAction("Error");

        Order? cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);
        if (cartOrder == null)
        {
            return RedirectToAction("Error");
        }

        OrderMenuItem? existingItem = cartOrder.OrderMenuItems?.FirstOrDefault(i => i.MenuItemId == menuItemId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            cartOrder.OrderMenuItems?.Add(new()
            {
                OrderId = cartOrder.Id,
                MenuItemId = menuItemId,
                Quantity = 1,
                Order = cartOrder,
                MenuItem = _context.MenuItems.First(m => m.Id == menuItemId)
            });
        }

        HttpContext.Session.SetObject($"cart_order_{userId}", cartOrder);

        return RedirectToAction("Index", new { selectedType, viewCart = true, reservationId });
    }


    // Remove menu item from the cart
    public IActionResult RemoveFromCart(Enums.OrderType selectedType, int menuItemId, int? reservationId)
    {
        int? userId = GetUserId();
        if (!userId.HasValue) return RedirectToAction("Error");

        Order? cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);
        if (cartOrder == null)
        {
            return RedirectToAction("Error");
        }

        OrderMenuItem? item = cartOrder.OrderMenuItems?.FirstOrDefault(c => c.MenuItemId == menuItemId);

        if (item != null)
        {
            if (item.Quantity > 1)
                item.Quantity--;
            else if (item.Quantity == 1)
                cartOrder.OrderMenuItems?.Remove(item);

            HttpContext.Session.SetObject($"cart_order_{userId}", cartOrder);
        }

        return RedirectToAction("Index", new { selectedType, viewCart = true, reservationId });
    }

    // Update quantity of an item in the cart
    [HttpPost]
    public IActionResult UpdateQuantity(int menuItemId, string action, OrderType type, int? reservationId)
    {
        int? userId = GetUserId();
        if (!userId.HasValue) return RedirectToAction("Error");

        Order cartOrder = HttpContext.Session.GetObject<Order>($"cart_order_{userId}")!;

        if (cartOrder == null || cartOrder.OrderMenuItems == null) return NotFound();

        OrderMenuItem? item = cartOrder.OrderMenuItems.FirstOrDefault(i => i.MenuItemId == menuItemId);

        if (item != null)
        {
            if (action == "increase") item.Quantity++;
            else if (action == "decrease" && item.Quantity > 1) item.Quantity--;
            else if (action == "decrease" && item.Quantity == 1) cartOrder.OrderMenuItems.Remove(item);

            HttpContext.Session.SetObject($"cart_order_{userId}", cartOrder);
        }

        return RedirectToAction("Index", "Order", new { selectedType = type, viewCart = true, reservationId });
    }

    // Calculate Subtotal
    private static decimal CalculateSubtotal(List<(int Quantity, decimal Price)> itemPrices)
        => itemPrices.Sum(item => item.Quantity * item.Price);

    // Calculate Tip
    private static decimal CalculateTip(string tipAmount, decimal? customTipAmount)
    {
        if (customTipAmount.HasValue)

            return Math.Round(customTipAmount.Value, 2);

        else if (decimal.TryParse(tipAmount, out decimal tip))
            return Math.Round(tip, 2);

        return 0;
    }

    // Calculate Tax 
    private static decimal CalculateTax(decimal subtotal, decimal? deliveryFee)
        => Math.Round((subtotal + deliveryFee ?? 0) * 0.05m, 2);

    // Calculate Delivery fee
    private static decimal? CalculateDeliveryFee(decimal subtotal, double deliveryDistanceKm, OrderType selectedType)
    {
        if (selectedType != OrderType.Delivery) return null;

        if (subtotal >= 75) return 0;
        if (deliveryDistanceKm <= 5) return 5.99m;
        if (deliveryDistanceKm <= 8) return 7.99m;
        if (deliveryDistanceKm <= 18) return 11.99m;
        if (deliveryDistanceKm <= 26) return 15.99m;

        throw new InvalidOperationException("Delivery is not available for distances over 26km.");
    }

    // Calculate Rewards 
    private static decimal CalculateRewardDiscount(bool redeemPoints, int availablePoints, out int pointsUsed)
    {
        decimal rewardsDiscount = 0.00m;
        pointsUsed = 0;


        if (redeemPoints && availablePoints >= 250)
        {
            rewardsDiscount = -20.00m;
            pointsUsed = 250;
        }

        return rewardsDiscount;
    }

    // Save order and user loyalty points
    private async Task SaveOrderAndUserPoints(Order order, int userId, int pointsUsed)
    {
        if (order.OrderMenuItems == null || order.OrderMenuItems.Count == 0)
            return;

        // Earn 1 point per $1 spent on subtotal
        int pointsEarned = (int)Math.Floor(order.Subtotal);

        User userInDb = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found.");

        userInDb.RewardsPoints += pointsEarned - pointsUsed;

        if (order.User != null && order.User.Id == userInDb.Id && order.User != userInDb)
            order.User = userInDb;

        // Save order changes
        if (order.Id == 0)
        {
            foreach (OrderMenuItem orderMenuItem in order.OrderMenuItems)
                _context.Attach(orderMenuItem.MenuItem);

            _context.Orders.Add(order);
        }
        else
            _context.Orders.Update(order);

        await _context.SaveChangesAsync();
    }

    public IActionResult Receipt(int orderId)
    {
        int? userId = GetUserId();

        if (userId == null)
            return RedirectToAction("Error");

        Order? cartOrder = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderMenuItems)!
                .ThenInclude(omi => omi.MenuItem)
            .Include(o => o.UserAddress)
            .Include(o => o.Reservation)
            .FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

        if (cartOrder == null)
            return RedirectToAction("Error");

        ViewBag.PointsEarned = (int)Math.Floor(cartOrder.Subtotal);
        ViewBag.SustainabilityMessage = "Thank you for choosing eco-friendly packaging. Together, we reduce waste.";

        string? discountStr = (string?)TempData["RewardsDiscount"];
        decimal rewardsDiscount = 0;

        if (!string.IsNullOrEmpty(discountStr))
            _ = decimal.TryParse(discountStr, out rewardsDiscount);

        ViewBag.RewardsDiscount = rewardsDiscount;

        return View(cartOrder);
    }

    // Error handling page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}