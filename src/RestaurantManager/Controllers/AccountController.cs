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
using System.Text.RegularExpressions;


namespace RestaurantManager.Controllers;

public class AccountController(ILogger<AccountController> logger, ApplicationDbContext _context) : Controller
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly ApplicationDbContext _context = _context;

    // GET: Account/Login
    public IActionResult Login(int userType)
    {
        ViewBag.UserType = userType;
        ViewBag.IsLogin = true;
        return View("Login");  // Return the Login view
    }

    // POST: Account/Login
    [HttpPost]
    public async Task<IActionResult> Login(User userInput, bool isInternalUserLogin = false)
    {
        ModelState.Remove("PasswordSalt");
        ModelState.Remove("FirstName");
        ModelState.Remove("LastName");
        ModelState.Remove("Phone");
        ModelState.Remove("isInternalUserLogin");

        if (ModelState.IsValid)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == userInput.Email);

            if (user != null && VerifyPassword(userInput.PasswordHash, user.PasswordHash!, user.PasswordSalt!))
            {
                if (user.Role == Enums.UserRole.Customer && isInternalUserLogin || user.Role != Enums.UserRole.Customer && !isInternalUserLogin)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                    return View("Login", userInput);
                }
                else
                {
                    // Create the identity with claims
                    List<Claim> claims =
                    [
                        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new(ClaimTypes.Name, user.Email),
                        new(ClaimTypes.Role, user.Role.ToString()),
                    ];

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
        ModelState.Remove("isInternalUserLogin");

        // --- PASSWORD VALIDATION ---
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            ModelState.AddModelError("PasswordHash", "Password is required.");
        }
        else
        {
            if (user.PasswordHash.Length < 8)
            {
                ModelState.AddModelError("PasswordHash", "Password must be at least 8 characters long.");
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
            if (!passwordRegex.IsMatch(user.PasswordHash))
            {
                ModelState.AddModelError("PasswordHash", "Password must include at least one uppercase letter, one lowercase letter, and one number.");
            }
        }

        // --- CONFIRM PASSWORD VALIDATION ---
        if (string.IsNullOrWhiteSpace(confirmPassword))
        {
            ModelState.AddModelError("confirmPassword", "Confirmation password is required.");
        }
        else if (user.PasswordHash != confirmPassword)
        {
            ModelState.AddModelError("confirmPassword", "Passwords do not match.");
        }

        // --- HANDLE IF VALID ---
        if (ModelState.IsValid)
        {
            try
            {
                // --- EMAIL VALIDATION ---
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    ModelState.AddModelError("Email", "Email is required.");
                }
                else if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("Email", "Invalid email format.");
                }
                else if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                }
                else
                {
                    try
                    {
                        // Generate salt + hash
                        var salt = GenerateSalt();
                        user.PasswordSalt = salt;
                        user.PasswordHash = HashPassword(user.PasswordHash!, salt);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error generating password hash.");
                        ModelState.AddModelError("", "An error occurred while securing your password.");
                        return View(user);
                    }

                    // Set default role
                    user.Role = Enums.UserRole.Customer;
                    try
                    {
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        Console.WriteLine("User saved: " + user.Email);
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to save user");
                        ModelState.AddModelError("", "An error occurred saving the user.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration.");
                ModelState.AddModelError("", "An unexpected error occurred during registration.");

            }
        }
        else
        {
            ModelState.AddModelError("", "Please insure all fields are filled out.");
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

    private int? GetUserId()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return null;

        Claim? userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
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
