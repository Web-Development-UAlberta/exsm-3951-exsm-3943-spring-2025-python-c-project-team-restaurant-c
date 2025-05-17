using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using Microsoft.AspNetCore.Authorization;

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
                                  .Include(o => o.OrderMenuItems!)
                                  .ThenInclude(omi => omi.MenuItem)
                                  .ToList();
        return View();
    }

    public IActionResult MenuItems()
    {
        ViewBag.MenuItems = _context.MenuItems.ToList();
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(MenuItem item)
    {
        if (ModelState.IsValid)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();
            return RedirectToAction("MenuItems");
        }

        return View(item);
    }

    public IActionResult Edit(int id)
    {
        var item = _context.MenuItems.Find(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(MenuItem item)
    {
        if (ModelState.IsValid)
        {
            _context.MenuItems.Update(item);
            _context.SaveChanges();
            return RedirectToAction("MenuItems");
        }

        return View(item);
    }

    public IActionResult Delete(int id)
    {
        var item = _context.MenuItems.Find(id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var item = _context.MenuItems.Find(id);
        if (item != null)
        {
            _context.MenuItems.Remove(item);
            _context.SaveChanges();
        }

        return RedirectToAction("MenuItems");
    }


    [HttpPost]
    public IActionResult UpdateOrderStatus(int orderId, Enums.OrderStatus status)
    {
        Order? order = _context.Orders.Find(orderId);

        if (order != null)
        {
            order.Status = status;
            _context.SaveChanges();
        }

        return RedirectToAction("Orders");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}