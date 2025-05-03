using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;


namespace RestaurantManager.Controllers;

[Authorize(Roles = "Admin, Staff, Manager")]
public class KitchenController(ILogger<KitchenController> logger) : Controller
{
    private readonly ILogger<KitchenController> _logger = logger;

    public IActionResult Dashboard()
    {
        var role = TempData["UserRole"] as string;

        if (role != "Admin")
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}