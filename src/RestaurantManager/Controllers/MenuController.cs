using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using System.Text.Json;

namespace RestaurantManager.Controllers;

public class MenuController : Controller
{
    private readonly IWebHostEnvironment _env;

    public MenuController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
