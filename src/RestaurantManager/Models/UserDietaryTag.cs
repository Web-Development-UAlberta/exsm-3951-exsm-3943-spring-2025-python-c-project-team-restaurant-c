using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.Models
{
  [Table("user_dietary_tag")]
  [PrimaryKey(nameof(UserId), nameof(TagId))]
  public class UserDietaryTag
  {

    [Column("user_id", TypeName = "INTEGER"), Required]
    public required int UserId { get; set; }

    [Column("tag_id", TypeName = "INTEGER"), Required]
    public required int TagId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("TagId")]
    public DietaryTag? DietaryTag { get; set; }
  }
}