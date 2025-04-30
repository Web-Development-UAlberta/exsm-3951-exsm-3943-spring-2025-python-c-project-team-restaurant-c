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

    public DbSet<Customer> Customers { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderMenuItem> OrderMenuItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuItemDietaryTag> MenuItemDietaryTags { get; set; }
    public DbSet<DietaryTag> DietaryTags { get; set; }
    public DbSet<UserDietaryTag> UserDietaryTags { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDietaryTag>()
            .HasKey(udt => new { udt.CustomerId, udt.TagId });

        modelBuilder.Entity<MenuItemDietaryTag>()
            .HasKey(mdt => new { mdt.MenuItemId, mdt.TagId });

        modelBuilder.Entity<OrderMenuItem>()
            .HasKey(omi => new { omi.OrderId, omi.MenuItemId });

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Order)
            .WithOne(o => o.Reservation)
            .HasForeignKey<Reservation>(r => r.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}

