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
    [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
    public required string Name { get; set; }

    [Column("description")]
    [Required]
    [StringLength(500, ErrorMessage = "Description must be under 500 characters.")]
    public required string Description { get; set; }

    [Column("price")]
    [Required]
    [Range(0.00, 20000, ErrorMessage = "Price must be between $0.01 and $20,000")]
    //There has to be a $20,000 bottle of something in the world, right?
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