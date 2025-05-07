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
    [Range(0, 100000, ErrorMessage = "Subtotal must be between $0 and $100,000.")]
    public required decimal Subtotal { get; set; }

    [Column("tax")]
    [Required]
    [Range(0, 10000, ErrorMessage = "Tax must be between $0 and $10,000")]
    public required decimal Tax { get; set; }

    [Column("tip_amount")]
    [Required]
    [Range(0, 100000, ErrorMessage = "Tip amount must be between $0 and $100,000.")]
    public required decimal? TipAmount { get; set; }

    [Column("total")]
    [Required]
    [Range(0, 250000, ErrorMessage = "Total must be between $250,000")]
    public required decimal? Total { get; set; }

    [Column("notes")]
    [StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
    public string? Notes { get; set; }

    [Column("delivery_fee")]
    [Range(0, 1000, ErrorMessage = "Delivery Fee must be under $1000")]
    public decimal? DeliveryFee { get; set; }

    [Column("delivery_instructions")]
    [StringLength(500, ErrorMessage = "Delivery Instructions must be under 500 characters.")]
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