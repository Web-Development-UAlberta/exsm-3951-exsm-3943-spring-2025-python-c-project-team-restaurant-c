using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.Models;

namespace RestaurantManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            ViewBag.IsLogin = true;
            return View("Login");  // Return the Login view
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            ViewBag.IsLogin = false;
            return View("Register");  // Return the Register view
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(User user)
        {
            // Implement login logic here (e.g., validate user credentials)
            if (ModelState.IsValid)
            {
                // Simulate login success, redirect to home/dashboard
                return RedirectToAction("Index", "Home");
            }

            ViewBag.IsLogin = true;
            return View("Login", user);  // Return the Login view again with validation errors
        }

        // POST: Account/Register
        [HttpPost]
        public IActionResult Register(User user, string confirmPassword)
        {
            // Implement registration logic here (e.g., validate passwords match)
            if (ModelState.IsValid && user.PasswordHash == confirmPassword)
            {
                // Simulate account creation, redirect to login page
                return RedirectToAction("Login");
            }

            ViewBag.IsLogin = false;
            return View("Register", user);  // Return the Register view again with validation errors
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
