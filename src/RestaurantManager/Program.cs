using RestaurantManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using DotNetEnv;
using Stripe;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
      options.LoginPath = "/Account/Login"; // Redirect here if not authenticated
      options.LogoutPath = "/Account/Logout";
      options.ExpireTimeSpan = TimeSpan.FromHours(1); // Keeps user logged in for 1 hour
      options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

Console.WriteLine("Using DB: " + connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

app.UseAuthentication();
app.UseAuthorization();

var supportedCultures = new[] { new CultureInfo("en-CA") };

var localizationOptions = new RequestLocalizationOptions
{
  DefaultRequestCulture = new RequestCulture("en-CA"),
  SupportedCultures = supportedCultures,
  SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
  var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  var userCount = context.Users.Count();
  var reservationCount = context.Reservations.Count();
  Console.WriteLine($"There are {userCount} users in the database.\n There are {reservationCount} reservations in the databse.");
}

app.Run();
