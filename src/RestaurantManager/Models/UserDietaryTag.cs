using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("user_dietary_tag")]
  public class UserDietaryTag
  {

    [Key, Column(Order = 0)]
    public required int UserId { get; set; }

    [Key, Column(Order = 1)]
    public required int TagId { get; set; }

    [ForeignKey("UserId")]
    public required User User { get; set; }

    [ForeignKey("TagId")]
    public required DietaryTag DietaryTag { get; set; }
  }
}