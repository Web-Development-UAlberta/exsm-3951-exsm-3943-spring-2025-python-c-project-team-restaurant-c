using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestaurantManager.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor that accepts ApplicationDbContext
        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index action that retrieves menu items with their dietary tags
        public async Task<IActionResult> Index()
        {
            // Fetch menu items with their dietary tags
            var menuItems = await _context.MenuItems
                .Include(m => m.MenuItemDietaryTags)
                    .ThenInclude(tag => tag.DietaryTag) // Include DietaryTag details
                .Where(m => m.IsAvailable) // Filter available menu items
                .ToListAsync();

            ViewBag.DietaryTags = await _context.DietaryTags.ToListAsync();

            return View(menuItems); // Pass the menu items to the view
        }

        // Error handling action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
