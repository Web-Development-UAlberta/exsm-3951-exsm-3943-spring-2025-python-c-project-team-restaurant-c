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
      [StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
      public required string Name { get; set; }

      [Column("description", TypeName = "VARCHAR(255)"), Required]
      [StringLength(500, ErrorMessage = "Description must be under 500 characters.")]
      public required string Description { get; set; }

      [Column("price", TypeName = "REAL"), Required]
      [Precision(10, 2)]
      [Range(0.01, 20000, ErrorMessage = "Price must be between $0.01 and $20,000.")]
      public required decimal Price { get; set; }

      [Column("category", TypeName = "INTEGER"), Required]
      public required MenuItemCategory Category { get; set; }

      [Column("is_available", TypeName = "INTEGER"), Required]
      public required bool IsAvailable { get; set; }

      public IEnumerable<OrderMenuItem>? OrderMenuItems { get; set; }
      public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
    }
}
