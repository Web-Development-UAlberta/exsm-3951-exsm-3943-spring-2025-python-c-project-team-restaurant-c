using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("reservation")]
  public class Reservation
  {
    [Key]
    [Column("id")]
    [Required]
    public required int Id { get; set; }

    [Column("user_id")]
    [Required]
    public required int UserId { get; set; }

    [Column("reservation_datetime")]
    [Required]
    public required DateTime ReservationDateTime { get; set; }

    [Column("guest_count")]
    [Required]
    [Range(1, 50, ErrorMessage = "Guest count may only be between 1 and 50.")]
    public required int GuestCount { get; set; }

    [Column("notes")]
    [StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
    public string? Notes { get; set; }

    [Column("status")]
    [Required]
    public required string Status { get; set; }

    [Column("table_number")]
    [Required]
    [Range(1, 30, ErrorMessage = "Table number must be between 1 and 30.")]
    public required int TableNumber { get; set; }

    [Column("created_at")]
    [Required]
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
  }
}