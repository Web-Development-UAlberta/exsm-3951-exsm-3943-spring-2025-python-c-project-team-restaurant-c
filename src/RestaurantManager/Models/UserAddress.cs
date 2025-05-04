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
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Address must be between 3 and 100 characters.")]
    public required string AddressLine1 { get; set; }

    [Column("address_line_2")]
    [StringLength(100, ErrorMessage = "Address must be under 100 characters.")]
    public string? AddressLine2 { get; set; }

    [Column("city")]
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "City must be under 50 characters.")]
    public required string City { get; set; }

    [Column("province")]
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Province must be under 50 characters.")]
    public required string Province { get; set; }

    [Column("postal_code")]
    [Required]
    //7 if they add - or space?
    [StringLength(7, ErrorMessage = "Postal Code cannot be over 7 characters.")]
    [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Invalid postal code format.")]
    public required string PostalCode { get; set; }

    [Column("country")]
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Country must be under 50 characters.")]
    public required string Country { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }
  }
}