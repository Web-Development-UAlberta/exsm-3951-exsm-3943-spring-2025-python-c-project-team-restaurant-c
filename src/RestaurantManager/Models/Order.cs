using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models
{
  [Table("order")]
  public class Order
  {

    [Key]
    [Column("id")]
    [Required]
    public required int Id { get; set; }

    [Column("user_id")]
    [Required]
    public required int UserId { get; set; }

    [Column("reservation_id")]
    public int? ReservationId { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [Column("order_type")]
    [Required]
    public required OrderType Type { get; set; }

    [Column("status")]
    [Required]
    public required OrderStatus Status { get; set; }

    [Column("subtotal")]
    [Required]
    public required decimal Subtotal { get; set; }

    [Column("tax")]
    [Required]
    public required decimal Tax { get; set; }

    [Column("tip_amount")]
    [Required]
    public required decimal? TipAmount { get; set; }

    [Column("total")]
    [Required]
    public required decimal? Total { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("delivery_fee")]
    public decimal? DeliveryFee { get; set; }

    [Column("delivery_instructions")]
    public string? DeliveryInstructions { get; set; }

    [Column("scheduled_time")]
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