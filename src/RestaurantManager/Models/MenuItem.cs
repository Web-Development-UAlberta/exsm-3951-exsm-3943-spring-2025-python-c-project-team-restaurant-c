using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

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
    public string? Category { get; set; }

    [Column("is_available")]
    public bool IsAvailable { get; set; }

    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
  }
}

//Do we need IsAvailable if we're not tracking inventory?