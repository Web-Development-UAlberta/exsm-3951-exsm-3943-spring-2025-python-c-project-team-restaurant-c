using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

  public class MenuItemDietaryTag
  {

    [Key, Column(Order = 0)]
    public int MenuItemId { get; set; }

    [Key, Column(Order = 1)]
    public int TagId { get; set; }

    [ForeignKey("MenuItemId")]
    public MenuItem? MenuItem { get; set; }

    [ForeignKey("TagId")]
    public DietaryTag? DietaryTag { get; set; }
  }
}