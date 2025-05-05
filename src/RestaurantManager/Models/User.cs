using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models;

[Table("user")]
public partial class User
{
  [Key]
  [Column("id")]
  [Required]
  public required int Id { get; set; }

  [Column("first_name")]
  [Required]
  public required string FirstName { get; set; }

  [Column("last_name")]
  [Required]
  public required string? LastName { get; set; }

  [Column("email")]
  [Required]
  public required string Email { get; set; }

  [Column("phone")]
  [Required]
  public required string Phone { get; set; }

  [Column("password_hash")]
  [Required]
  public required string PasswordHash { get; set; }

  [Column("password_salt")]
  public required string PasswordSalt { get; set; }

  [Column("rewards_points")]
  public required int RewardsPoints { get; set; }

  [Column("dietary_notes")]
  public string? DietaryNotes { get; set; }

  [Column("role")]
  public required UserRole Role { get; set; }

  public IEnumerable<UserAddress>? UserAddresses { get; set; }

  public IEnumerable<Reservation>? Reservations { get; set; }

  public IEnumerable<Order>? Orders { get; set; }

  public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
}