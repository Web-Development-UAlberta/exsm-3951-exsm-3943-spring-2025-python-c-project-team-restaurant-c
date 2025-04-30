using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;

namespace RestaurantManager.Data;

public class ApplicationDbContext : IdentityDbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {
  }

  public DbSet<User> Users { get; set; }
  public DbSet<UserAddress> UserAddresses { get; set; }
  public DbSet<PaymentMethod> PaymentMethods { get; set; }
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderMenuItem> OrderMenuItems { get; set; }
  public DbSet<MenuItem> MenuItems { get; set; }
  public DbSet<MenuItemDietaryTag> MenuItemDietaryTags { get; set; }
  public DbSet<DietaryTag> DietaryTags { get; set; }
  public DbSet<UserDietaryTag> UserDietaryTags { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<UserDietaryTag>()
        .HasKey(udt => new { udt.UserId, udt.TagId });

    // Seed MenuItems
    modelBuilder.Entity<MenuItem>().HasData(
        new MenuItem { Id = 1, Name = "Vegan Pizza", Description = "A delicious vegan pizza with gluten-free crust.", Price = 12.99M },
        new MenuItem { Id = 2, Name = "Chicken Wrap", Description = "A tasty chicken wrap with fresh vegetables.", Price = 9.99M },
        new MenuItem { Id = 3, Name = "Caesar Salad", Description = "A classic Caesar salad with creamy dressing.", Price = 7.99M }
    );

    // Seed DietaryTags
    modelBuilder.Entity<DietaryTag>().HasData(
        new DietaryTag { Id = 1, Name = "Vegan" },
        new DietaryTag { Id = 2, Name = "Gluten-Free" },
        new DietaryTag { Id = 3, Name = "Dairy-Free" }
    );

    // Seed MenuItemDietaryTags
    modelBuilder.Entity<MenuItemDietaryTag>(entity =>
    {
      entity.HasKey(m => new { m.MenuItemId, m.TagId });
      entity.HasData(
        new MenuItemDietaryTag { MenuItemId = 1, TagId = 1 },
        new MenuItemDietaryTag { MenuItemId = 1, TagId = 2 },
        new MenuItemDietaryTag { MenuItemId = 2, TagId = 3 }
        );
    });

    // Seed OrderMenuItems (Example Order data)
    modelBuilder.Entity<OrderMenuItem>(entity =>
    {
      entity.HasKey(omi => new { omi.OrderId, omi.MenuItemId });
    });


    base.OnModelCreating(modelBuilder);
  }
}

