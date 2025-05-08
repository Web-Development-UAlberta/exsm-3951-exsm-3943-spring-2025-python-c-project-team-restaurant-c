using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
    [Table("dietary_tag")]
    public class DietaryTag
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      [Column("id", TypeName = "INTEGER"), Required]
      public required int Id { get; set; }

      [Column("name", TypeName = "VARCHAR(100)"), Required]
      [StringLength(100, ErrorMessage = "Dietary Tag Name must be under 100 characters.")]
      public required string Name { get; set; }

      public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
      public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
    }
}