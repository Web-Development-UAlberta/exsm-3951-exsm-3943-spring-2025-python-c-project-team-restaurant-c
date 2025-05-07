using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Models;
using RestaurantManager.Enums;

namespace RestaurantManager.Data;

public class ApplicationDbContext : IdentityDbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
  {
  }

  public new DbSet<User> Users { get; set; }
  public DbSet<UserAddress> UserAddresses { get; set; }
  public DbSet<Reservation> Reservations { get; set; }
  public DbSet<Order> Orders { get; set; }
  public DbSet<OrderMenuItem> OrderMenuItems { get; set; }
  public DbSet<MenuItem> MenuItems { get; set; }
  public DbSet<MenuItemDietaryTag> MenuItemDietaryTags { get; set; }
  public DbSet<DietaryTag> DietaryTags { get; set; }
  public DbSet<UserDietaryTag> UserDietaryTags { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
        .Property(u => u.Role)
        .HasConversion<string>();

    modelBuilder.Entity<UserDietaryTag>()
          .HasKey(udt => new
          {
            udt.UserId,
            udt.TagId
          });

    modelBuilder.Entity<Order>()
        .Property(o => o.Status)
        .HasConversion<string>();

    modelBuilder.Entity<Order>()
        .Property(o => o.Type)
        .HasConversion<string>();

    // Seed MenuItems
    modelBuilder.Entity<MenuItem>(entity =>
    {
      entity.Property(m => m.Category)
        .HasConversion<string>();

      entity.HasData(
        // MainCourse
        new MenuItem { Id = 1, Name = "Vegan Pizza", Description = "A delicious vegan pizza with gluten-free crust.", Price = 12.99M, Category = MenuItemCategory.MainCourse, IsAvailable = true },
        new MenuItem { Id = 2, Name = "Chicken Wrap", Description = "A tasty chicken wrap with fresh vegetables.", Price = 9.99M, Category = MenuItemCategory.MainCourse, IsAvailable = true },
        new MenuItem { Id = 3, Name = "Caesar Salad", Description = "A classic Caesar salad with creamy dressing.", Price = 7.99M, Category = MenuItemCategory.MainCourse, IsAvailable = true },
        new MenuItem { Id = 4, Name = "Beef Lasagna", Description = "Hearty beef lasagna with mozzarella cheese.", Price = 13.49M, Category = MenuItemCategory.MainCourse, IsAvailable = true },
        new MenuItem { Id = 5, Name = "Grilled Salmon", Description = "Grilled salmon with lemon butter sauce.", Price = 15.99M, Category = MenuItemCategory.MainCourse, IsAvailable = true },

        // Appetizers
        new MenuItem { Id = 6, Name = "Garlic Bread", Description = "Toasted garlic bread with herbs.", Price = 4.99M, Category = MenuItemCategory.Appetizer, IsAvailable = true },
        new MenuItem { Id = 7, Name = "Bruschetta", Description = "Tomatoes, garlic, basil on toasted baguette.", Price = 5.99M, Category = MenuItemCategory.Appetizer, IsAvailable = true },
        new MenuItem { Id = 8, Name = "Stuffed Mushrooms", Description = "Mushrooms filled with cheese and herbs.", Price = 6.99M, Category = MenuItemCategory.Appetizer, IsAvailable = true },
        new MenuItem { Id = 9, Name = "Spring Rolls", Description = "Vegetable spring rolls with sweet chili sauce.", Price = 5.49M, Category = MenuItemCategory.Appetizer, IsAvailable = true },
        new MenuItem { Id = 10, Name = "Nachos", Description = "Loaded nachos with cheese and jalapeños.", Price = 8.99M, Category = MenuItemCategory.Appetizer, IsAvailable = true },

        // Desserts
        new MenuItem { Id = 11, Name = "Chocolate Cake", Description = "Rich chocolate cake with fudge frosting.", Price = 6.99M, Category = MenuItemCategory.Dessert, IsAvailable = true },
        new MenuItem { Id = 12, Name = "Cheesecake", Description = "Creamy cheesecake with a graham cracker crust.", Price = 6.49M, Category = MenuItemCategory.Dessert, IsAvailable = true },
        new MenuItem { Id = 13, Name = "Ice Cream Sundae", Description = "Vanilla ice cream with toppings.", Price = 5.99M, Category = MenuItemCategory.Dessert, IsAvailable = true },
        new MenuItem { Id = 14, Name = "Tiramisu", Description = "Italian dessert with coffee and mascarpone.", Price = 7.49M, Category = MenuItemCategory.Dessert, IsAvailable = true },
        new MenuItem { Id = 15, Name = "Fruit Salad", Description = "Fresh seasonal fruit medley.", Price = 4.99M, Category = MenuItemCategory.Dessert, IsAvailable = true },

        // Beverages
        new MenuItem { Id = 16, Name = "Coffee", Description = "Freshly brewed coffee.", Price = 2.99M, Category = MenuItemCategory.Beverage, IsAvailable = true },
        new MenuItem { Id = 17, Name = "Iced Tea", Description = "Chilled black tea with lemon.", Price = 2.99M, Category = MenuItemCategory.Beverage, IsAvailable = true },
        new MenuItem { Id = 18, Name = "Smoothie", Description = "Blended fruit smoothie with yogurt.", Price = 4.99M, Category = MenuItemCategory.Beverage, IsAvailable = true },
        new MenuItem { Id = 19, Name = "Lemonade", Description = "Fresh squeezed lemonade.", Price = 3.49M, Category = MenuItemCategory.Beverage, IsAvailable = true },
        new MenuItem { Id = 20, Name = "Soda", Description = "Choice of Coke, Sprite, or Root Beer.", Price = 2.49M, Category = MenuItemCategory.Beverage, IsAvailable = true }
      );
    });

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

    modelBuilder.Entity<MenuItemDietaryTag>()
        .HasOne(m => m.MenuItem)
        .WithMany(m => m.MenuItemDietaryTags)
        .HasForeignKey(m => m.MenuItemId);

    modelBuilder.Entity<MenuItemDietaryTag>()
        .HasOne(m => m.DietaryTag)
        .WithMany(t => t.MenuItemDietaryTags)
        .HasForeignKey(m => m.TagId);

    // Seed OrderMenuItems (Example Order data)
    modelBuilder.Entity<OrderMenuItem>(entity =>
    {
      entity.HasKey(omi => new { omi.OrderId, omi.MenuItemId });
    });

    // See User
    modelBuilder.Entity<User>(entity =>
    {
      entity.HasData(
        new User { Id = 2, FirstName = "Admin", LastName = "Test", Email = "admin@gmail.com", Phone = "111-111-1111", PasswordHash = "+1fzEaaKIt+hxB8eZ5RK6sywJXqY5Qkn7CxNoG6ckxc=", PasswordSalt = "hHA0qDG/iJSbc9PEUXJ8UQ==", RewardsPoints = 0, Role = UserRole.Admin });
    });

    base.OnModelCreating(modelBuilder);
  }
}

