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
    public IActionResult SubmitCheckout(int selectedAddressId, Enums.OrderType selectedType, decimal? CustomTipAmount, string TipAmount, double deliveryDistanceKm = 0, bool redeemPoints = false)
    {
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

        // Determine the final tip amount
        decimal finalTipAmount;
        if (TipAmount == "other" && CustomTipAmount.HasValue)
        {
            finalTipAmount = CustomTipAmount.Value;
        }
        else if (decimal.TryParse(TipAmount, out var parsedTip))
        {
            finalTipAmount = parsedTip;
        }
        else
        {
            finalTipAmount = 0;
        }

        cartOrder.TipAmount = finalTipAmount;
        cartOrder.OrderDate = DateTime.UtcNow;

        if (selectedType == Enums.OrderType.Delivery)
        {
            cartOrder.AddressId = selectedAddressId;
        }

        // Handle address logic (optional)
        var selectedAddress = _context.UserAddresses.FirstOrDefault(a => a.Id == selectedAddressId);
        if (selectedAddress != null)
        {
            // Optionally process or validate the address
        }

        // Calculate total
        decimal total = CalculateOrderTotal(cartOrder, finalTipAmount, deliveryDistanceKm, selectedType);

        // Rewards logic
        if (redeemPoints && user.RewardsPoints >= 250)
        {
            total = Math.Max(0, total - 20);
            user.RewardsPoints -= 250;
        }

        // Save the order and update user points
        SaveOrderAndUserPoints(cartOrder, total, finalTipAmount, selectedType, user);

        // Remove cart from session
        HttpContext.Session.Remove($"cart_order_{userId}");

        return RedirectToAction("Receipt", new { orderId = cartOrder.Id });
    }

    public IActionResult Receipt(int orderId)
    {
        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Error");

        Order? cartOrder = _context.Orders
     .Include(o => o.OrderMenuItems)!
         .ThenInclude(omi => omi.MenuItem)
     .FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

        if (cartOrder == null) return RedirectToAction("Error");

        ViewBag.PointsEarned = (int)Math.Floor(cartOrder.Subtotal);
        ViewBag.SustainabilityMessage = "Thank you for choosing eco-friendly packaging. Together, we reduce waste.";

        return View(cartOrder);
    }


    // Method to get or create a cart order for the user
    private Order? GetOrCreateCartOrder(int userId, Enums.OrderType selectedType)
    {
        Order cartOrder = HttpContext.Session.GetObject<Order>($"cart_order_{userId}")!;

        if (cartOrder == null)
        {
            User? user = _context.Users.Find(userId);
            if (user == null) return null;

            cartOrder = new()
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

    // Calculate total for the checkout
    private static decimal CalculateOrderTotal(Order cartOrder, decimal tipAmount, double deliveryDistanceKm, Enums.OrderType selectedType)
    {
        decimal subtotal = cartOrder.OrderMenuItems!.Sum(item => item.Quantity * item.MenuItem.Price);
        decimal deliveryFee = CalculateDeliveryFee(subtotal, deliveryDistanceKm, selectedType);
        decimal tax = Math.Round((subtotal + deliveryFee + tipAmount) * 0.05m, 2);
        return subtotal + deliveryFee + tipAmount + tax;
    }

    // Calculate delivery fee
    private static decimal CalculateDeliveryFee(decimal subtotal, double deliveryDistanceKm, Enums.OrderType selectedType)
    {
        if (selectedType != Enums.OrderType.Delivery) return 0;

        if (subtotal >= 75) return 0;
        if (deliveryDistanceKm <= 5) return 5.99m;
        if (deliveryDistanceKm <= 8) return 7.99m;
        return 0; // Or handle outside delivery range
    }

    // Save order and user loyalty points
    private void SaveOrderAndUserPoints(Order cartOrder, decimal total, decimal tipAmount, Enums.OrderType selectedType, User user)
    {
        cartOrder.Subtotal = total - tipAmount;
        cartOrder.TipAmount = tipAmount;
        cartOrder.Total = total;
        cartOrder.Status = Enums.OrderStatus.Confirmed; // Final confirmed status
        cartOrder.OrderDate = DateTime.UtcNow;
        cartOrder.UserId = user.Id;

        int pointsEarned = (int)Math.Floor(cartOrder.Subtotal);
        user.RewardsPoints += pointsEarned;

        _context.Orders.Add(cartOrder);

        if (cartOrder.OrderMenuItems != null)
        {
            foreach (var item in cartOrder.OrderMenuItems)
            {
                item.OrderId = cartOrder.Id; // Link to order
                _context.OrderMenuItems.Add(item);
            }
        }

        // _context.Users.Update(user);
        _context.SaveChanges();
    }


    // Error handling page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
