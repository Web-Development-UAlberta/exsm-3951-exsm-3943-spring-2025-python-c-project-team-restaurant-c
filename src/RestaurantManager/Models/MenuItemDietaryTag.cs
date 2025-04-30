using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

    public class MenuItemDietaryTag
    {

        [Key, Column(Order = 0)]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("DietaryTag")]
        public int TagId { get; set; }

        public MenuItem? MenuItem { get; set; }
        public DietaryTag? DietaryTag { get; set; }
    }
}