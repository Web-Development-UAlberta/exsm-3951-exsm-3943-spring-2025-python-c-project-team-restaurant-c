using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("payment_method")]
  public class PaymentMethod
  {

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("type")]
    public string? Type { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("payment_processor_token")]
    public string PaymentProcessorToken { get; set; }

    [Column("expiry_date")]
    public DateTime ExpiryDate { get; set; }

    [Column("postal_code")]
    public string? PostalCode { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
  }
}