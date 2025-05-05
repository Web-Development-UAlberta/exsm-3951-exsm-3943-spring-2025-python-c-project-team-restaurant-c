using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;


namespace RestaurantManager.Controllers;

[Authorize(Roles = "Admin, Staff, Manager")]
public class KitchenDashboardController(ILogger<KitchenDashboardController> logger) : Controller
{
    private readonly ILogger<KitchenDashboardController> _logger = logger;

    public IActionResult Index()
    {
        // var role = TempData["UserRole"] as string;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}