using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;

namespace RestaurantManager.Controllers;

public class MenuController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IActionResult> Index(string tag = "all")
    {
        IQueryable<MenuItem> query = _context.MenuItems
            .Where(m => m.IsAvailable)
            .Include(m => m.MenuItemDietaryTags)
                .ThenInclude(md => md.DietaryTag);

        if (tag != "all")
        {
            query = query.Where(m =>
                m.MenuItemDietaryTags != null &&
                m.MenuItemDietaryTags.Any(md =>
                    md.DietaryTag != null &&
                    md.DietaryTag.Name.Equals(tag)));
        }

        List<MenuItem> menuItems = await query.ToListAsync();

        ViewBag.DietaryTags = await _context.DietaryTags.ToListAsync();

        return View(menuItems);
    }

    // Error handling action
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
