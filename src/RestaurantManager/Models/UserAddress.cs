using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("user_address")]
  public class UserAddress
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("address_line_1")]
    public string? AddressLine1 { get; set; }

    [Column("address_line_2")]
    public string? AddressLine2 { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("province")]
    public string? Province { get; set; }

    [Column("postal_code")]
    public string? PostalCode { get; set; }

    [Column("country")]
    public string? Country { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
  }
}