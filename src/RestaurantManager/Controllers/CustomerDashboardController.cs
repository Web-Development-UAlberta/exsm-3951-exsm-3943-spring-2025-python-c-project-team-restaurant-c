using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;
using System.Security.Claims;

namespace RestaurantManager.Data;

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
        user.Orders = user.Orders?.OrderByDescending(o => o.OrderDate).Take(2).ToList();

        if (user == null)
            return NotFound();

        //Only show reservations that are Booked or Seated
        user.Reservations = _context.Reservations
            .Where(o => o.UserId == user.Id && (o.ReservationStatus == Enums.ReservationStatus.Booked || o.ReservationStatus == Enums.ReservationStatus.Seated))
            .OrderByDescending(o => o.ReservationDateTime)
            .ToList();

        user.Orders = user.Orders!.OrderByDescending(o => o.OrderDate).Take(2).ToList();

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
            .Include(o => o.OrderMenuItems)
                .ThenInclude(omi => omi.MenuItem)
            .Include(o => o.Reservation)
            .ToListAsync();

        return View(orders);
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
