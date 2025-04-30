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

    modelBuilder.Entity<MenuItemDietaryTag>()
        .HasKey(mdt => new { mdt.MenuItemId, mdt.TagId });

    modelBuilder.Entity<OrderMenuItem>()
        .HasKey(omi => new { omi.OrderId, omi.MenuItemId });

    base.OnModelCreating(modelBuilder);
  }
}

