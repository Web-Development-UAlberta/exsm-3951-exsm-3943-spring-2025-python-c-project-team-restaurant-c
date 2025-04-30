using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestaurantManager.Models
{

  public class Order
  {

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("reservation_id")]
    public int ReservationId { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [Column("order_type")]
    public string? OrderType { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("tip_amount")]
    public decimal? TipAmount { get; set; }

    [Column("total_amount")]
    public decimal? TotalAmount { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("delivery_fee")]
    public decimal? DeliveryFee { get; set; }

    [Column("delivery_instructions")]
    public string? DeliveryInstructions { get; set; }

    [Column("scheduled_time")]
    public DateTime? ScheduledTime { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("ReservationId")]
    public Reservation? Reservation { get; set; }

    [ForeignKey("AddressId")]
    public UserAddress? UserAddress { get; set; }
    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
  }
}