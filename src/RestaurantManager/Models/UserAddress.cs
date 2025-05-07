using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("user_address")]
  public class UserAddress
  {
    [Key]
    [Column("id")]
    [Required]
    public required int Id { get; set; }

    [Column("user_id")]
    [Required]
    public required int UserId { get; set; }

    [Column("address_line_1")]
    [Required]
    public required string AddressLine1 { get; set; }

    [Column("address_line_2")]
    public string? AddressLine2 { get; set; }

    [Column("city")]
    [Required]
    public required string City { get; set; }

    [Column("province")]
    [Required]
    public required string Province { get; set; }

    [Column("postal_code")]
    [Required]
    public required string PostalCode { get; set; }

    [Column("country")]
    [Required]
    public required string Country { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }
  }
}