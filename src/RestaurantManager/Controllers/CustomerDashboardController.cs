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
            .Include(u => u.Orders!.OrderByDescending(o => o.OrderDate))
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
            return NotFound();

        user.Orders = user.Orders!.OrderByDescending(o => o.OrderDate).Take(2).ToList();

        return View(user);
    }

    public IActionResult OrderHistory()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var orders = _context.Orders
            .Include(o => o.User)
            .Include(o => o.UserAddress)
            .Include(o => o.Reservation)
            .Where(o => o.User.Email == email)
            .OrderByDescending(o => o.OrderDate)
            .Include(o => o.OrderMenuItems)

            .ToList();

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
