using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models;

[Table("user")]
public partial class User
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  [Column("id", TypeName = "INTEGER"), Required, Key]
  public required int Id { get; set; }

  [Column("first_name", TypeName = "VARCHAR(255)"), Required]
  public required string FirstName { get; set; }

  [Column("last_name", TypeName = "VARCHAR(255)"), Required]
  public required string LastName { get; set; }

  [Column("email", TypeName = "VARCHAR(255)"), Required, EmailAddress]
  public required string Email { get; set; }

  [Column("phone", TypeName = "VARCHAR(50)"), Required, Phone]
  public required string Phone { get; set; }

  [Column("password_hash", TypeName = "VARCHAR(255)"), Required]
  public required string PasswordHash { get; set; }

  [Column("password_salt", TypeName = "VARCHAR(255)"), Required]
  public required string PasswordSalt { get; set; }

  [Column("rewards_points", TypeName = "INTEGER"), Required]
  public required int RewardsPoints { get; set; }

  [Column("dietary_notes", TypeName = "TEXT")]
  public string? DietaryNotes { get; set; }

  [Column("role", TypeName = "INTEGER"), Required]
  public required UserRole Role { get; set; }

  public IEnumerable<UserAddress>? UserAddresses { get; set; }

  public IEnumerable<Reservation>? Reservations { get; set; }

  public IEnumerable<Order>? Orders { get; set; }

  public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
}