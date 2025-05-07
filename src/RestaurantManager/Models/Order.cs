using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using RestaurantManager.Enums;

namespace RestaurantManager.Models
{
  [Table("order")]
  public class Order
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "INTEGER"), Required, Key]
    public required int Id { get; set; }

    [Column("user_id", TypeName = "INTEGER"), Required]
    public required int UserId { get; set; }

    [Column("reservation_id", TypeName = "INTEGER")]
    public int? ReservationId { get; set; }

    [Column("address_id", TypeName = "INTEGER")]
    public int? AddressId { get; set; }

    [Column("order_type", TypeName = "INTEGER"), Required]
    public required OrderType Type { get; set; }

    [Column("status", TypeName = "INTEGER"), Required]
    public required OrderStatus Status { get; set; }

    [Column("subtotal", TypeName = "REAL"), Required, Precision(10, 2)]
    public required decimal Subtotal { get; set; }

    [Column("tax", TypeName = "REAL"), Required, Precision(10, 2)]
    public required decimal Tax { get; set; }

    [Column("tip_amount", TypeName = "REAL"), Required, Precision(10, 2)]
    public required decimal TipAmount { get; set; }

    [Column("total_amount", TypeName = "REAL"), Required, Precision(10, 2)]
    public required decimal Total { get; set; }

    [Column("notes", TypeName = "TEXT")]
    public string? Notes { get; set; }

    [Column("delivery_fee", TypeName = "REAL"), Precision(10, 2)]
    public decimal? DeliveryFee { get; set; }

    [Column("delivery_instructions", TypeName = "TEXT")]
    public string? DeliveryInstructions { get; set; }

    [Column("scheduled_time", TypeName = "TEXT")]
    public DateTime? ScheduledTime { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }

    [ForeignKey("ReservationId")]
    public Reservation? Reservation { get; set; }

    [ForeignKey("AddressId")]
    public UserAddress? UserAddress { get; set; }
    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
  }
}