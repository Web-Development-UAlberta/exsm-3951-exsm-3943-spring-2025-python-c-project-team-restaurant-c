using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("menu_item_dietary_tag")]
  public class MenuItemDietaryTag
  {

    [ForeignKey("MenuItemId")]
    public int MenuItemId { get; set; }

    [ForeignKey("TagId")]
    public int TagId { get; set; }

    public MenuItem? MenuItem { get; set; }

    public DietaryTag? DietaryTag { get; set; }
  }
}