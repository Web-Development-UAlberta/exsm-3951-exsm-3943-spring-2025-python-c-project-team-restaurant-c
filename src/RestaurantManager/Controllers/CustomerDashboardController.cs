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
        {
            return Unauthorized(); // Not logged in
        }

        var user = _context.Users
            .Include(u => u.UserAddresses)
            .Include(u => u.Reservations)
            .Include(u => u.UserDietaryTags)
            .FirstOrDefault(u => u.Email == email); // Match by email


        if (user == null)
        {
            return NotFound();
        }

        return View(user);
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
