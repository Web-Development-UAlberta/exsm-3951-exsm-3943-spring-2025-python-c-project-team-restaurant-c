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
            .Where(r => r.ReservationStatus != Enums.ReservationStatus.Cancelled
                && r.ReservationDateTime >= DateTime.Now)
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
        //Only show reservations that are in the future and not cancelled
        //Sort by the soonest first
        ViewBag.Reservations = _context.Reservations
            .Include(r => r.User)
            .Where(r => r.ReservationStatus != Enums.ReservationStatus.Cancelled
                    && r.ReservationDateTime >= DateTime.Now)  // Add this line!
            .OrderBy(r => r.ReservationDateTime)
            .ToList();


        //Only show reservations that have passed or been cancelled.
        ViewBag.PastReservations = _context.Reservations
            .Include(r => r.User)
            .Where(r => r.ReservationStatus == Enums.ReservationStatus.Cancelled
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateReservationStatus(int reservationId, string newStatus)
    {

        //Get the reservation that we're updating
        var reservation = _context.Reservations.Find(reservationId);

        //Check if the reservation exists and it has a valid status
        if (reservation != null && Enum.TryParse<Enums.ReservationStatus>(newStatus, out var status))
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