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
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("category")]
    public MenuItemCategory Category { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }

    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
  }
}

//Do we need IsAvailable if we're not tracking inventory?