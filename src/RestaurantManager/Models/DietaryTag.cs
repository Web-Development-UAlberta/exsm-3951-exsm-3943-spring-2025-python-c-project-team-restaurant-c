using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

    public class DietaryTag
    {

        [Key]
        public int TagId { get; set; }

        public string? Name { get; set; }

        public List<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
        public List<UserDietaryTag>? UserDietaryTags { get; set; }
    }
}