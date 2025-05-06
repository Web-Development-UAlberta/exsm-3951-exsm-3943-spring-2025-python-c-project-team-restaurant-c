using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models
{
  [Table("reservation")]
  public class Reservation
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "INTEGER"), Required, Key]
    public required int Id { get; set; }

    [Column("user_id", TypeName = "INTEGER"), Required]
    public required int UserId { get; set; }

    [Column("reservation_datetime", TypeName = "TEXT"), Required]
    public required DateTime ReservationDateTime { get; set; }

    [Column("guest_count", TypeName = "INTEGER"), Required]
    public required int GuestCount { get; set; }

    [Column("notes", TypeName = "TEXT")]
    public string? Notes { get; set; }

    [Column("status", TypeName = "INTEGER"), Required]
    public required OrderStatus Status { get; set; }

    [Column("table_number", TypeName = "INT"), Required]
    public required int TableNumber { get; set; }

    [Column("created_at", TypeName = "TEXT"), Required]
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at", TypeName = "TEXT")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
  }
}