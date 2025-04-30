using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

  public class DietaryTag
  {

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
    public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
  }
}