using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("user_address")]
  public class UserAddress
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "INTEGER"), Key, Required]
    public required int Id { get; set; }

    [Column("user_id", TypeName = "INTEGER"), Required]
    public required int UserId { get; set; }

    [Column("address_line_1", TypeName = "VARCHAR(255)"), Required]
    public required string AddressLine1 { get; set; }

    [Column("address_line_2", TypeName = "VARCHAR(255)")]
    public string? AddressLine2 { get; set; }

    [Column("city", TypeName = "VARCHAR(100)"), Required]
    public required string City { get; set; }

    [Column("province", TypeName = "VARCHAR(100)"), Required]
    public required string Province { get; set; }

    [Column("postal_code", TypeName = "VARCHAR(20)"), Required]
    public required string PostalCode { get; set; }

    [Column("country", TypeName = "VARCHAR(100)"), Required]
    public required string Country { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }
  }
}