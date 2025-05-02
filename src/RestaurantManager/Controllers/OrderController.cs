using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using Microsoft.AspNetCore.Authorization;


namespace RestaurantManager.Controllers;

[Authorize]
public class OrderController : Controller
{

    public IActionResult Order()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
