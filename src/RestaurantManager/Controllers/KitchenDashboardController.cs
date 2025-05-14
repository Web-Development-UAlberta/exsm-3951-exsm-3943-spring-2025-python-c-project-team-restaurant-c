using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantManager.Utilities;

namespace RestaurantManager.Controllers;

[Authorize(Roles = "Admin, Staff, Manager")]

public class KitchenDashboardController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index(Enums.OrderType? selectedType)
    {
        var reservations = _context.Reservations
            .Where(r => r.ReservationDateTime >= DateTime.Now)
            .ToList();

        var orders = _context.Orders
            .Where(o => o.Status == Enums.OrderStatus.Pending || o.Status == Enums.OrderStatus.InProgress)
            .ToList();

        var menuItems = _context.MenuItems
            .Where(m => m.IsAvailable)
            .ToList();

        ViewBag.UpcomingReservations = reservations.Count;
        ViewBag.ActiveOrders = orders.Count;
        ViewBag.AvailableMenuItems = menuItems.Count;

        return View();
    }

    public IActionResult Reservations()
    {
        ViewBag.Reservations = _context.Reservations.Include(r => r.User).ToList();
        return View();
    }

    public IActionResult Orders()
    {
        ViewBag.Orders = _context.Orders
                                  .Include(o => o.OrderMenuItems)
                                  .ThenInclude(omi => omi.MenuItem)
                                  .ToList();
        return View();
    }

    public IActionResult MenuItems()
    {
        ViewBag.MenuItems = _context.MenuItems.ToList();
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}