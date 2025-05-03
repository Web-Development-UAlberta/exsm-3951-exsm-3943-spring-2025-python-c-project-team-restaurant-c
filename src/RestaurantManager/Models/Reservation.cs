using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("reservation")]
  public class Reservation
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("reservation_datetime")]
    public DateTime ReservationDateTime { get; set; }

    [Column("guest_count")]
    public int GuestCount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("table_number")]
    public int? TableNumber { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
  }
}