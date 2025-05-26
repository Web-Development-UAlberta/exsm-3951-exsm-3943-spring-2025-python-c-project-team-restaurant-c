using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.Models
{
    [Table("menu_item_dietary_tag")]
    [PrimaryKey(nameof(MenuItemId), nameof(TagId))]
    public class MenuItemDietaryTag
    {
      [Column("menu_item_id", TypeName = "INTEGER"), Required]
      public required int MenuItemId { get; set; }

      [Column("tag_id", TypeName = "INTEGER"), Required]
      public required int TagId { get; set; }

      [ForeignKey("MenuItemId")]
      public MenuItem? MenuItem { get; set; }

      [ForeignKey("TagId")]
      public DietaryTag? DietaryTag { get; set; }
    }
}
