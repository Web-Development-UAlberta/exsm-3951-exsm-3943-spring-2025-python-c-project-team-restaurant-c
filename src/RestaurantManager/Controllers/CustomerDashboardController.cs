using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;
using System.Security.Claims;
using RestaurantManager.Data;

namespace RestaurantManager.Controllers;

[Authorize]
public class CustomerDashboardController(ILogger<CustomerDashboardController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<CustomerDashboardController> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var user = _context.Users
            .Include(u => u.UserAddresses)
            .Include(u => u.Reservations)
            .Include(u => u.UserDietaryTags)
            .Include(u => u.Orders)
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
            return NotFound();

        user.Orders = user.Orders?.OrderByDescending(o => o.OrderDate).Take(2).ToList();

        user.Orders = user.Orders!.OrderByDescending(o => o.OrderDate).Take(2).ToList();

        ViewData["CurrentRoute"] = "CustomerDashboard";

        return View(user);
    }

    private int? GetUserId()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

        Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
    }

    public async Task<IActionResult> OrderHistory()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderMenuItems!)
                .ThenInclude(omi => omi.MenuItem)
            .Include(o => o.Reservation)
            .ToListAsync();

        return View(orders);
    }

    [HttpPost]
    public IActionResult UpdateAddress(UserAddress address)
    {
        ModelState.Remove("User");

        if (!ModelState.IsValid)
            return RedirectToAction("Index", "CustomerDashboard");

        int? userId = GetUserId();

        if (userId == null)
            return RedirectToAction("Login");

        if (address.Id == 0)
        {
            address.UserId = (int)userId;
            _context.UserAddresses.Add(address);
        }
        else
        {
            // Update existing
            var existing = _context.UserAddresses.FirstOrDefault(a => a.Id == address.Id && a.UserId == (int)userId);
            if (existing == null) return NotFound();

            existing.AddressLine1 = address.AddressLine1;
            existing.AddressLine2 = address.AddressLine2;
            existing.City = address.City;
            existing.Province = address.Province;
            existing.PostalCode = address.PostalCode;
            existing.Country = address.Country;
        }

        _context.SaveChanges();
        return RedirectToAction("Index", "CustomerDashboard");
    }

    [HttpPost]
    public IActionResult UpdateUserInfo(User updatedUser)
    {
        ModelState.Remove("PasswordHash");

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "CustomerDashboard");
        }

        int? userId = GetUserId();

        if (userId == null)
            return RedirectToAction("Login");

        var userInDb = _context.Users.FirstOrDefault(u => u.Id == (int)userId);
        if (userInDb == null)
        {
            return NotFound();
        }

        userInDb.FirstName = updatedUser.FirstName;
        userInDb.LastName = updatedUser.LastName;
        userInDb.Email = updatedUser.Email;
        userInDb.Phone = updatedUser.Phone;

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction("Index", "CustomerDashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
