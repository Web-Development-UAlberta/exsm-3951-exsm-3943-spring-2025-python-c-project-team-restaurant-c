using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestaurantManager.Models;
using System.Security.Claims;
using RestaurantManager.Data;
using System.Text;
using System.Security.Cryptography;

namespace RestaurantManager.Controllers;

[Authorize]
public class CustomerDashboardController(ILogger<CustomerDashboardController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<CustomerDashboardController> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public IActionResult Index()
    {
        var user = LoadFullUser(); // Use the helper to load the user with all related data
        if (user == null)
            return Unauthorized(); // or RedirectToAction("Login") if more appropriate

        ViewData["CurrentRoute"] = "CustomerDashboard";
        return View(user);
    }

    private int? GetUserId()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

        Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
    }

    public async Task<IActionResult> OrderHistory()
    {
        var userId = GetUserId();
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderMenuItems!)
                .ThenInclude(omi => omi.MenuItem)
            .Include(o => o.Reservation)
            .ToListAsync();

        return View(orders);
    }

    [HttpPost]
    public IActionResult UpdateAddress(UserAddress address)
    {
        ModelState.Remove("User");

        if (!ModelState.IsValid)
            return RedirectToAction("Index", "CustomerDashboard");

        int? userId = GetUserId();

        if (userId == null)
            return RedirectToAction("Login");

        if (address.Id == 0)
        {
            address.UserId = (int)userId;
            _context.UserAddresses.Add(address);
        }
        else
        {
            // Update existing
            var existing = _context.UserAddresses.FirstOrDefault(a => a.Id == address.Id && a.UserId == (int)userId);
            if (existing == null) return NotFound();

            existing.AddressLine1 = address.AddressLine1;
            existing.AddressLine2 = address.AddressLine2;
            existing.City = address.City;
            existing.Province = address.Province;
            existing.PostalCode = address.PostalCode;
            existing.Country = address.Country;
        }

        _context.SaveChanges();
        return RedirectToAction("Index", "CustomerDashboard");
    }

    [HttpPost]
    public IActionResult UpdateUserInfo(User updatedUser)
    {
        ModelState.Remove("PasswordHash");

        if (!ModelState.IsValid)
        {
            var fullUser = LoadFullUser();
            if (fullUser == null)
                return Unauthorized();

            // Copy over the posted values so they appear in the form again
            fullUser.FirstName = updatedUser.FirstName;
            fullUser.LastName = updatedUser.LastName;
            fullUser.Email = updatedUser.Email;
            fullUser.Phone = updatedUser.Phone;

            ViewData["CurrentRoute"] = "CustomerDashboard";
            return View("Index", fullUser);
        }

        int? userId = GetUserId();
        if (userId == null) return RedirectToAction("Login");

        var userInDb = _context.Users.FirstOrDefault(u => u.Id == (int)userId);
        if (userInDb == null) return NotFound();

        userInDb.FirstName = updatedUser.FirstName;
        userInDb.LastName = updatedUser.LastName;
        userInDb.Email = updatedUser.Email;
        userInDb.Phone = updatedUser.Phone;

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction("Index", "CustomerDashboard");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    private User? LoadFullUser()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            return null;

        var user = _context.Users
            .Include(u => u.UserAddresses)
            .Include(u => u.Reservations)
            .Include(u => u.UserDietaryTags)
            .Include(u => u.Orders)
            .FirstOrDefault(u => u.Email == email);

        if (user == null)
            return null;

        user.Reservations = _context.Reservations
            .Where(r => r.UserId == user.Id
                && (r.ReservationStatus == Enums.ReservationStatus.Booked || r.ReservationStatus == Enums.ReservationStatus.Seated)
                && r.ReservationDateTime >= DateTime.Now)
            .OrderBy(r => r.ReservationDateTime)
            .ToList();

        user.Orders = user.Orders?.OrderByDescending(o => o.OrderDate).Take(2).ToList();

        return user;
    }

    // Password hashing with salt (SHA256)
    private static string HashPassword(string password, string salt)
    {
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = SHA256.HashData(combined);
        return Convert.ToBase64String(hash);
    }

    // Salt generation
    private static string GenerateSalt()
    {
        var bytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    // Password verification
    private static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var enteredHash = HashPassword(enteredPassword, storedSalt);
        return storedHash == enteredHash;
    }

    [HttpPost]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ChangePasswordModel = model;

            var fullUser = LoadFullUser();
            return View("Index", fullUser);
        }

        int? userId = GetUserId();
        if (userId == null) return NotFound();

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return NotFound();

        if (!VerifyPassword(model.CurrentPassword, user.PasswordHash, user.PasswordSalt!))
        {
            ModelState.AddModelError("CurrentPassword", "The current password is incorrect.");

            ViewBag.ChangePasswordModel = model;

            var fullUser = LoadFullUser();
            return View("Index", fullUser);
        }

        // Update password
        user.PasswordSalt = GenerateSalt();
        user.PasswordHash = HashPassword(model.NewPassword, user.PasswordSalt);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Password updated successfully.";
        return RedirectToAction("Index");
    }
}
