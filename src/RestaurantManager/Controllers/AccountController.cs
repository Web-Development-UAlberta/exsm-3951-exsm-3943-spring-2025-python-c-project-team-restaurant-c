using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;
using RestaurantManager.Data;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RestaurantManager.Controllers;

public class AccountController(ILogger<AccountController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    // GET: Account/Login
    public IActionResult Login(int userType)
    {
        ViewBag.UserType = userType;
        ViewBag.IsLogin = true;
        return View("Login");  // Return the Login view
    }

    // POST: Account/Login
    [HttpPost]
    public async Task<IActionResult> Login(User userInput)
    {
        ModelState.Remove("PasswordSalt");
        ModelState.Remove("FirstName");
        ModelState.Remove("LastName");
        ModelState.Remove("Phone");

        if (ModelState.IsValid)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userInput.Email);
            if (user != null && VerifyPassword(userInput.PasswordHash, user.PasswordHash!, user.PasswordSalt!))
            {
                // Create the identity with claims
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Email),
                    new(ClaimTypes.Role, user.Role.ToString()),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // keeps the user logged in
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // if ((userInput.Role == Enums.UserRole.Customer && ViewBag.UserType == 0) ||
                //     ((userInput.Role == Enums.UserRole.Admin || userInput.Role == Enums.UserRole.Manager || userInput.Role == Enums.UserRole.Staff) && ViewBag.UserType == 1))

                // Redirect based on role
                return user.Role switch
                {
                    Enums.UserRole.Admin or Enums.UserRole.Staff or Enums.UserRole.Manager => RedirectToAction("Index", "KitchenDashboard"),
                    _ => RedirectToAction("Index", "Home"),
                };
            }

            ModelState.AddModelError(string.Empty, "Invalid login credentials.");
        }
        else
        {
            foreach (KeyValuePair<string, ModelStateEntry?> m in ModelState.Where(m => m.Value?.ValidationState == ModelValidationState.Invalid))
            {
                ModelErrorCollection? errors = m.Value?.Errors;
                if (errors != null && errors.Any())
                {
                    foreach (var error in errors)
                        ModelState.AddModelError(string.Empty, $"{m.Key} failed validation: {error.ErrorMessage}");
                }
            }
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

                Console.WriteLine("User saved: " + user.Email);

                return RedirectToAction("Login");
            }
        }

        ViewBag.IsLogin = false;
        return View("Register", user);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

}
