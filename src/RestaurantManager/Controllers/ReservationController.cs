using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;
using RestaurantManager.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Enums;

namespace RestaurantManager.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Reservation/Index 
    public IActionResult Index()
    {
        var userId = GetLoggedInUserId();
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        Console.WriteLine("GET Index called - Just showing the form");

        //Return view with default reservation
        //Will be properly editied below
        var reservation = new Reservation
        {
            Id = 0,
            UserId = userId,
            ReservationDateTime = DateTime.Now.AddHours(1),
            GuestCount = 1,
            ReservationStatus = ReservationStatus.Booked,
            TableNumber = 1,
            CreatedAt = DateTime.UtcNow
        };

        //Prepopulate fields with logged in user's credentials
        ViewBag.FirstName = user!.FirstName;
        ViewBag.LastName = user!.LastName;
        ViewBag.Email = user!.Email;
        ViewBag.Phone = user!.Phone;

        return View(reservation);

    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SubmitReservation(Reservation reservation)
    {
        Console.WriteLine("POST SubmitReservation called - Processing form submission");
        
        //Remove fields that shouldn't be validated from form input
        ModelState.Remove("TableNumber");
        ModelState.Remove("ReservationStatus");
        ModelState.Remove("UserId");
        
        if (ModelState.IsValid)
        {
            Console.WriteLine("SUBMIT STATE VALID - Saving reservation");
            
            //Set system-generated fields
            reservation.UserId = GetLoggedInUserId();
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.TableNumber = GetAvailableTable(reservation.ReservationDateTime, reservation.GuestCount);
            
            //Check table availability
            if (IsTableAvailable(reservation.TableNumber, reservation.GuestCount, reservation.ReservationDateTime))
            {
                reservation.ReservationStatus = ReservationStatus.Booked;
                
                //Save to database
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                Console.WriteLine($"Reservation saved with ID: {reservation.Id}");
                
                TempData["Message"] = "Reservation submitted successfully!";
                return RedirectToAction("Confirmation");
            }
            else
            {
                Console.WriteLine("No tables available");
                ModelState.AddModelError("", "Sorry, no tables are available at this time.");
            }
        }
        else
        {
            Console.WriteLine("SUBMIT STATE INVALID - Validation errors:");
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    Console.WriteLine($"- Error in {state.Key}: {state.Value.Errors[0].ErrorMessage}");
                }
            }
        }
        
        return View("Index", reservation);
    }

    private int GetLoggedInUserId()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
        {
            throw new InvalidOperationException("User email is missing or invalid.");
        }
        
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new InvalidOperationException("User not found with the provided email.");
        }
        
        return user.Id;
    }


    private int GetAvailableTable(DateTime reservationDateTime, int guestCount)
    {
        //First, fetch all non-cancelled reservations for the day
        var startOfDay = reservationDateTime.Date;
        var endOfDay = startOfDay.AddDays(1);
        
        var dayReservations = _context.Reservations
            .Where(r => r.ReservationDateTime >= startOfDay 
                && r.ReservationDateTime < endOfDay
                && r.ReservationStatus != ReservationStatus.Cancelled)
            .ToList(); // Execute query and bring results to memory
        
        //Now filter in memory for reservations within 2 hours
        var conflictingReservations = dayReservations
            .Where(r => Math.Abs((r.ReservationDateTime - reservationDateTime).TotalHours) < 2)
            .ToList();
        
        //Get occupied tables
        var reservedTables = conflictingReservations
            .Select(r => r.TableNumber)
            .ToList();
        
        //Find appropriate table based on party size
        int startTable, endTable;
        
        if (guestCount <= 2)
        {
            startTable = 1;
            endTable = 10;
        }
        else if (guestCount <= 6)
        {
            startTable = 11;
            endTable = 20;
        }
        else
        {
            startTable = 21;
            endTable = 30;
        }
        
        //Find first available table in preferred range
        for (int tableNum = startTable; tableNum <= endTable; tableNum++)
        {
            if (!reservedTables.Contains(tableNum))
            {
                return tableNum;
            }
        }
        
        //Try any table if preferred range is full
        for (int tableNum = 1; tableNum <= 30; tableNum++)
        {
            if (!reservedTables.Contains(tableNum))
            {
                return tableNum;
            }
        }
        
        // No tables available
        return -1;
    }

    private bool IsTableAvailable(int tableNumber, int guestCount, DateTime reservationDateTime)
    {
        //First, fetch all non-cancelled reservations for the day
        var startOfDay = reservationDateTime.Date;
        var endOfDay = startOfDay.AddDays(1);
        
        var dayReservations = _context.Reservations
            .Where(r => r.ReservationDateTime >= startOfDay 
                && r.ReservationDateTime < endOfDay
                && r.ReservationStatus != ReservationStatus.Cancelled)
            .ToList(); // Execute query and bring results to memory
        
        //Filter for reservations within 2 hours
        var conflictingReservations = dayReservations
            .Where(r => Math.Abs((r.ReservationDateTime - reservationDateTime).TotalHours) < 2)
            .ToList();
        
        //Check total guests
        int currentTotalGuests = conflictingReservations.Sum(r => r.GuestCount);
        if (currentTotalGuests + guestCount > 50)
        {
            return false;
        }
        
        //Check if table is already reserved
        bool isTableReserved = conflictingReservations.Any(r => r.TableNumber == tableNumber);
        return !isTableReserved;
    }

    public IActionResult Confirmation()
    {
        //Check if we have a reservation ID in TempData
        if (TempData["ReservationId"] != null && int.TryParse(TempData["ReservationId"]!.ToString(), out int reservationId))
        {
            //Get the specific reservation
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation != null)
            {
                return View(reservation);
            }
        }
        
        //Fallback: get the most recent reservation for the current user
        var userId = GetLoggedInUserId();
        var latestReservation = _context.Reservations
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefault();
        
        return View(latestReservation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult BookAndConfirm(Reservation reservation)
    {
        Console.WriteLine("BookAndConfirm method called directly");
        
        if (reservation.ReservationDateTime <= DateTime.Now)
{
            ModelState.AddModelError("ReservationDateTime", "Please select a date/time in the future.");
            return View("Index", reservation); 
        }
        
        try
        {
            //Fill in the automatic details
            reservation.UserId = GetLoggedInUserId();
            reservation.CreatedAt = DateTime.UtcNow;
            reservation.TableNumber = GetAvailableTable(reservation.ReservationDateTime, reservation.GuestCount);
            reservation.ReservationStatus = ReservationStatus.Booked;

            //Save to database
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            Console.WriteLine($"Reservation saved with ID: {reservation.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        //Redirect to confirmation
        return RedirectToAction("Confirmation");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteReservation(int id)
    {
        //Get current user's email and find the reservation that belongs to them
        var email = User.Identity?.Name;
        var reservation = _context.Reservations
            .Include(r => r.User)
            .FirstOrDefault(r => r.Id == id && r.User!.Email == email);

        //Mark the reservation as cancelled and update the UpdatedAt
        reservation!.ReservationStatus = Enums.ReservationStatus.Cancelled;
        reservation.UpdatedAt = DateTime.UtcNow;

        //Save changes and go back to Dashboard
        _context.SaveChanges();

        return RedirectToAction("Index", "CustomerDashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditReservation(Reservation editedReservation){

        //Get current user's email and find the reservation that belongs to them
        var email = User.Identity?.Name;
        var existingReservation = _context.Reservations
            .Include(r => r.User)
            .FirstOrDefault(r => r.Id == editedReservation.Id && r.User!.Email == email);

        //Make sure the reservation exists
        if (existingReservation != null)
        {
            //Update the DateTime, # Guests, Special Notes, UpdatedAt and check if they need a new Table
            existingReservation.ReservationDateTime = editedReservation.ReservationDateTime;
            existingReservation.GuestCount = editedReservation.GuestCount;
            existingReservation.Notes = editedReservation.Notes;
            existingReservation.UpdatedAt = DateTime.UtcNow;

            existingReservation.TableNumber = GetAvailableTable(editedReservation.ReservationDateTime, editedReservation.GuestCount);

            _context.SaveChanges();
        }

        return RedirectToAction("Index", "CustomerDashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
