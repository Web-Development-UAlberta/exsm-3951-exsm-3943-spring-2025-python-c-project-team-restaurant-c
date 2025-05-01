using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;

namespace RestaurantManager.Controllers;

public class MenuController : Controller
{
    private readonly ApplicationDbContext _context;

    public MenuController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var menuItems = _context.MenuItems
        .Include(m => m.MenuItemDietaryTags)
            .ThenInclude(tagLink => tagLink.DietaryTag)
        .ToList();

        return View(menuItems);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
