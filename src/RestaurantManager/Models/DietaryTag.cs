using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("dietary_tag")]
  public class DietaryTag
  {

    [Key]
    [Column("id")]
    [Required]
    public required int Id { get; set; }

    [Column("name")]
    [Required]
    public required string Name { get; set; }

    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
    public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
  }
}