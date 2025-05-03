using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using RestaurantManager.Data;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext _context;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            ViewBag.IsLogin = true;
            return View("Login");  // Return the Login view
        }
        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(User userInput)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == userInput.Email);
                if (user != null && VerifyPassword(userInput.PasswordHash, user.PasswordHash!, user.PasswordSalt!))
                {
                    // Redirect based on role
                    if (user.Role == Enums.UserRole.Admin)
                    {
                        return RedirectToAction("Dashboard", "Kitchen");
                    }
                    else if (user.Role == Enums.UserRole.Staff)
                    {
                        return RedirectToAction("Dashboard", "Staff");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login credentials.");
            }

            ViewBag.IsLogin = true;
            return View("Login", userInput);
        }


        // GET: Account/Register
        public IActionResult Register()
        {
            ViewBag.IsLogin = false;
            return View("Register");  // Return the Register view
        }


        // POST: Account/Register
        [HttpPost]
        public IActionResult Register(User user, string confirmPassword)
        {
            if (ModelState.IsValid && user.PasswordHash == confirmPassword)
            {
                if (user.PasswordHash != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                }
                else if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                }
                else
                {
                    // Generate salt + hash
                    var salt = GenerateSalt();
                    user.PasswordSalt = salt;
                    user.PasswordHash = HashPassword(user.PasswordHash!, salt);

                    // Set default role
                    user.Role = Enums.UserRole.Customer;
                    try
                    {
                        _context.Users.Add(user);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to save user");
                        ModelState.AddModelError("", "An error occurred saving the user.");
                    }


                    Console.WriteLine("✅ User saved: " + user.Email);

                    return RedirectToAction("Login");
                }
            }

            ViewBag.IsLogin = false;
            return View("Register", user);
        }

        // Password hashing with salt (SHA256)
        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        // Salt generation
        private string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        // Password verification
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var enteredHash = HashPassword(enteredPassword, storedSalt);
            return storedHash == enteredHash;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}