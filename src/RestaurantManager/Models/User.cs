using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models;

[Table("user")]
public partial class User
{
  [Key]
  [Column("id")]
  public int Id { get; set; }

  [Column("first_name")]
  public string FirstName { get; set; } = string.Empty;

  [Column("last_name")]
  public string LastName { get; set; } = string.Empty;

  [Column("email")]
  public string? Email { get; set; }

  [Column("phone")]
  public string? Phone { get; set; }

  [Column("password_hash")]
  public string? PasswordHash { get; set; }

  [Column("password_salt")]
  public string? PasswordSalt { get; set; }

  [Column("rewards_points")]
  public int RewardsPoints { get; set; }

  [Column("dietary_notes")]
  public string? DietaryNotes { get; set; }

  [Column("role")]
  public UserRole Role { get; set; }

  public IEnumerable<PaymentMethod>? PaymentMethods { get; set; }

  public IEnumerable<UserAddress>? UserAddresses { get; set; }

  public IEnumerable<Reservation>? Reservations { get; set; }

  public IEnumerable<Order>? Orders { get; set; }

  public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
}