using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Data;
using RestaurantManager.Enums;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantManager.Controllers;

[Authorize(Roles = "Admin, Staff, Manager")]

public class KitchenDashboardController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index(OrderType? selectedType)
    {
        var reservations = _context.Reservations
            .Where(r => r.ReservationStatus != ReservationStatus.Cancelled
                && r.ReservationDateTime >= DateTime.Now)
            .ToList();

        var orders = _context.Orders
            .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InProgress)
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
        //Only show reservations that are in the future and not cancelled
        //Sort by the soonest first
        ViewBag.Reservations = _context.Reservations
            .Include(r => r.User)
            .Where(r => r.ReservationStatus != ReservationStatus.Cancelled
                    && r.ReservationDateTime >= DateTime.Now)  // Add this line!
            .OrderBy(r => r.ReservationDateTime)
            .ToList();


        //Only show reservations that have passed or been cancelled.
        ViewBag.PastReservations = _context.Reservations
            .Include(r => r.User)
            .Where(r => r.ReservationStatus == ReservationStatus.Cancelled
                    || r.ReservationDateTime < DateTime.Now.AddHours(-1))
            .OrderByDescending(r => r.ReservationDateTime)
            .ToList();

        return View();
    }

    public IActionResult Orders()
    {
        ViewBag.Orders = _context.Orders
                                .Include(o => o.User)
                                .Include(o => o.OrderMenuItems!)
                                .ThenInclude(omi => omi.MenuItem)
                                .Include(o => o.UserAddress)
                                .Include(o => o.Reservation)
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
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }

        if (item.Price <= 0)
        {
            ModelState.AddModelError("Price", "Price must be greater than zero.");
        }

        if (!Enum.IsDefined(typeof(MenuItemCategory), item.Category))
        {
            ModelState.AddModelError("Category", "Please select a valid category.");
        }

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the errors below and try again.";
            return View(item);
        }

        if (ModelState.IsValid)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();
            return RedirectToAction("MenuItems");
        }

        return View(item);
    }

    [HttpGet]
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
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            ModelState.AddModelError("Name", "Name is required.");
        }

        if (item.Price <= 0)
        {
            ModelState.AddModelError("Price", "Price must be greater than zero.");
        }

        if (!Enum.IsDefined(typeof(MenuItemCategory), item.Category))
        {
            ModelState.AddModelError("Category", "Please select a valid category.");
        }

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Please correct the errors below and try again.";
            return View(item);
        }
        var existingItem = _context.MenuItems.FirstOrDefault(m => m.Id == item.Id);
        if (existingItem == null)
        {
            return NotFound();
        }

        try
        {
            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Category = item.Category;
            existingItem.Price = item.Price;
            existingItem.IsAvailable = item.IsAvailable;

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Menu item updated successfully!";
            return RedirectToAction("MenuItems");
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving. Please try again.");
            TempData["ErrorMessage"] = "An unexpected error occurred.";
            return View(item);
        }
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
    public IActionResult UpdateOrderStatus(int orderId, OrderStatus status)
    {
        Order? order = _context.Orders.Find(orderId);

        if (order != null)
        {
            order.Status = status;
            _context.SaveChanges();
        }

        return RedirectToAction("Orders");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateReservationStatus(int reservationId, string newStatus)
    {

        //Get the reservation that we're updating
        var reservation = _context.Reservations.Find(reservationId);

        //Check if the reservation exists and it has a valid status
        if (reservation != null && Enum.TryParse<ReservationStatus>(newStatus, out var status))
        {
            //Change status to the new one
            //Mark when it happened then save to the db
            reservation.ReservationStatus = status;
            reservation.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }

        return RedirectToAction("Reservations");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteReservation(int reservationId)
    {

        //Get the reservation that we're deleting
        var reservation = _context.Reservations.Find(reservationId);

        //If the reservation exists
        if (reservation != null)
        {
            //Delete it then save the change to db
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }

        return RedirectToAction("Reservations");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}