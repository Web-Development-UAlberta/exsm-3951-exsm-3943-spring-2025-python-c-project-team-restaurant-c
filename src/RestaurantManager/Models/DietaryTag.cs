using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("dietary_tag")]
  public class DietaryTag
  {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id", TypeName = "INTEGER"), Required, Key]
    public required int Id { get; set; }

    [Column("name", TypeName = "VARCHAR(100)"), Required]
    public required string Name { get; set; }

    public IEnumerable<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
    public IEnumerable<UserDietaryTag>? UserDietaryTags { get; set; }
  }
}