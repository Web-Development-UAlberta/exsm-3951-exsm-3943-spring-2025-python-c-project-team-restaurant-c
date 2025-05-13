using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;
using RestaurantManager.Data;
using System.Security.Claims;

namespace RestaurantManager.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            reservation.UserId = GetLoggedInUserId();

            // Automatically set the CreatedAt field
            reservation.CreatedAt = DateTime.UtcNow;

            // Set TableNumber - Here would check the availability of tables
            reservation.TableNumber = GetAvailableTable();  // Implement this method to fetch an available table number

            // Set Status based on table availability and guest count
            if (IsTableAvailable(reservation.TableNumber, reservation.GuestCount))  // Implement table availability logic
            {
                reservation.ReservationStatus = Enums.ReservationStatus.Seated;
            }
            else
            {
                reservation.ReservationStatus = Enums.ReservationStatus.Seated;
            }

            // Save to the database
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Confirmation");
        }
        return View(reservation);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitReservation(Reservation reservation)
    {
        ModelState.Remove("TableNumber");

        if (ModelState.IsValid)
        {
            // TODO: Save reservation or process it
            TempData["Message"] = "Reservation submitted successfully!";
            return RedirectToAction("Confirmation");
        }

        return View("Index", reservation);
    }


    // methods for getting UserId, available tables, and checking table availability
    private int GetLoggedInUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userId, out int id))
        {
            return id;
        }
        throw new InvalidOperationException("User ID claim is missing or invalid.");
    }


    private int GetAvailableTable()
    {
        // Implement logic to fetch an available table from the database or a predefined list
        // For now, assuming table 1 is available
        return 1; // Placeholder for actual table selection logic
    }

    /*
        private int? GetAvailableTable(DateTime reservationTime, int guestCount)
        {
            var availableTable = _context.Tables
                .Where(t => t.Capacity >= guestCount &&
                            !_context.Reservations.Any(r =>
                                r.TableNumber == t.TableNumber &&
                                r.ReservationDateTime == reservationTime))
                .OrderBy(t => t.Capacity)
                .FirstOrDefault();

            return availableTable?.TableNumber;
        }
    */

    private bool IsTableAvailable(int tableNumber, int guestCount)
    {
        // Implement the logic to check if the table is available for the given guest count
        // For now, assuming all tables are available
        return true; // Placeholder for actual availability check
    }


    public IActionResult Confirmation()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
