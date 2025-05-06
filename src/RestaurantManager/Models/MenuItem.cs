using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using RestaurantManager.Enums;

namespace RestaurantManager.Models
{
  [Table("menu_item")]
  public class MenuItem
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "INTEGER"), Required, Key]
    public required int Id { get; set; }

    [Column("name", TypeName = "VARCHAR(255)"), Required]
    public required string Name { get; set; }

    [Column("description", TypeName = "VARCHAR(255)"), Required]
    public required string Description { get; set; }

    [Column("price", TypeName = "REAL"), Precision(10, 2)]
    [Required]
    public required decimal Price { get; set; }

    [Column("category", TypeName = "INTEGER"), Required]
    public required MenuItemCategory Category { get; set; }

    [Column("is_available", TypeName = "INTEGER"), Required]
    public required bool IsAvailable { get; set; }

    public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
  }
}

//Do we need IsAvailable if we're not tracking inventory?