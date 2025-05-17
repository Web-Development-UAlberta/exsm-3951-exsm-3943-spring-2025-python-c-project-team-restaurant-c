using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantManager.Utilities;

namespace RestaurantManager.Controllers;

[Authorize]
public class OrderController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // Index Action to load menu items and optionally view the cart
    public async Task<IActionResult> Index(Enums.OrderType? selectedType, bool viewCart = false, string tag = "all")
    {
        int? userId = GetUserId();
        if (userId == null)
            return RedirectToAction("Login");  // Redirect to login if no user is found 

        selectedType ??= Enums.OrderType.TakeOut;

        List<MenuItem> menuItems = await GetMenuItemsWithTags(tag);
        List<DietaryTag> dietaryTags = [.. _context.DietaryTags];
        Order? cart = GetOrCreateCartOrder(userId.Value, selectedType.Value);

        ViewBag.DietaryTags = dietaryTags;
        ViewBag.OrderMenuItems = cart?.OrderMenuItems;
        ViewBag.Cart = cart;
        ViewBag.ViewCart = viewCart;
        ViewBag.SelectedType = selectedType;

        return View(menuItems);
    }

    // Get the user ID from claims
    private int? GetUserId()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

        Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
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
    public IActionResult SetOrderType(Enums.OrderType selectedType)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        var cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);

        // Include the selectedType as a query parameter in the redirect
        return RedirectToAction("Index", new { selectedType = selectedType });
    }

    // Action to view cart in a partial
    public IActionResult Cart(Enums.OrderType selectedType)
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
    public IActionResult Checkout(Enums.OrderType selectedType)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        Order? cartOrder = GetOrCreateCartOrder(userId.Value, selectedType);
        if (cartOrder == null || cartOrder.OrderMenuItems == null || cartOrder.OrderMenuItems.Count == 0)
        {
            return RedirectToAction("Index", "Order"); // Show error or redirect if cart is empty
        }

        return View(cartOrder); // Redirect to confirmation page
    }

    [HttpPost]
    public async Task<IActionResult> SubmitCheckout(
        int selectedAddressId,
        Enums.OrderType selectedType,
        decimal? customTipAmount,
        string tipAmount,
        double deliveryDistanceKm = 0,
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

        // Set order type
        cartOrder.Type = selectedType;

        // Order Date
        cartOrder.OrderDate = DateTime.UtcNow;

        // Calculate Subtotal 
        cartOrder.Subtotal = CalculateSubtotal([.. cartOrder.OrderMenuItems!.Select(i => (i.Quantity, i.MenuItem.Price))]);

        // Calculate Delivery FeeaqW@S
        cartOrder.DeliveryFee = CalculateDeliveryFee(cartOrder.Subtotal, deliveryDistanceKm, selectedType);

        // Calculate Tax 
        cartOrder.Tax = CalculateTax(cartOrder.Subtotal, cartOrder.DeliveryFee);

        // Calculate Tip 
        decimal tip = CalculateTip(tipAmount, customTipAmount);
        cartOrder.TipAmount = tip;

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

        // Delivery Address (Optional)
        if (selectedType == Enums.OrderType.Delivery)
        {
            cartOrder.AddressId = selectedAddressId;
        }

        var selectedAddress = _context.UserAddresses.FirstOrDefault(a => a.Id == selectedAddressId && a.UserId == userId);
        if (selectedAddress != null)
        {
            // Optionally process or validate the address
        }

        // Save the order and update user points
        await SaveOrderAndUserPoints(cartOrder, user.Id, pointsUsed);

        // Remove cart from session
        HttpContext.Session.Remove($"cart_order_{userId}");

        TempData["RewardsDiscount"] = rewardsDiscount.ToString();

        return RedirectToAction("Receipt", new { orderId = cartOrder.Id });
    }

    // Method to get or create a cart order for the user
    private Order? GetOrCreateCartOrder(int userId, Enums.OrderType selectedType)
    {
        Order? cartOrder = HttpContext.Session.GetObject<Order>($"cart_order_{userId}");

        if (cartOrder == null)
        {
            User? user = _context.Users.Find(userId);
            if (user == null) return null;

            cartOrder = new Order
            {
                Status = Enums.OrderStatus.InProgress,
                UserId = userId,
                User = user,
                Type = selectedType,
                Subtotal = 0,
                Tax = 0,
                TipAmount = 0,
                Total = 0,
                OrderDate = DateTime.Now,
                OrderMenuItems = []
            };
        }

        cartOrder.Type = selectedType;
        HttpContext.Session.SetObject($"cart_order_{userId}", cartOrder);
        return cartOrder;
    }

    // Add menu item to the cart
    public IActionResult AddToCart(Enums.OrderType selectedType, int menuItemId)
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

        return RedirectToAction("Index", new { selectedType, viewCart = true });
    }

    // Remove menu item from the cart
    public IActionResult RemoveFromCart(Enums.OrderType selectedType, int menuItemId)
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

        return RedirectToAction("Index", new { selectedType, viewCart = true });
    }

    // Update quantity of an item in the cart
    [HttpPost]
    public IActionResult UpdateQuantity(int menuItemId, string action)
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

        return RedirectToAction("Index", "Order", new { cartOrder.Type, viewCart = true });
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
    private static decimal CalculateDeliveryFee(decimal subtotal, double deliveryDistanceKm, Enums.OrderType selectedType)
    {
        if (selectedType != Enums.OrderType.Delivery) return 0;

        if (subtotal >= 75) return 0;
        if (deliveryDistanceKm <= 5) return 5.99m;
        if (deliveryDistanceKm <= 8) return 7.99m;

        throw new InvalidOperationException("Delivery is not available for distances over 8km.");
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
