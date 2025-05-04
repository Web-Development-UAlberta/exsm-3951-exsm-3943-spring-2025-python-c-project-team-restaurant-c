using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManager.Enums;

namespace RestaurantManager.Models
{
  [Table("menu_item")]
  public class MenuItem
  {

    [Key]
    [Column("id")]
    [Required]
    public required int Id { get; set; }

    [Column("name")]
    [Required]
    public required string Name { get; set; }

    [Column("description")]
    [Required]
    public required string Description { get; set; }

    [Column("price")]
    [Required]
    public required decimal Price { get; set; }

    [Column("category")]
    [Required]
    public required MenuItemCategory Category { get; set; }

    [Column("is_available")]
    [Required]
    public required bool IsAvailable { get; set; }

    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
  }
}

//Do we need IsAvailable if we're not tracking inventory?