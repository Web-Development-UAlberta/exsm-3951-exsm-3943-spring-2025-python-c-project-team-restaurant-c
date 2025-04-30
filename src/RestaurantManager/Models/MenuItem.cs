using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

    public class MenuItem
    {

        [Key]
        public int MenuItemId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Category { get; set; }
        public bool IsAvailable { get; set; }

        public List<OrderMenuItem>? OrderMenuItems { get; set; }
        public List<MenuItemDietaryTag>? MenuItemDietaryTags { get; set; }
    }
}

//Do we need IsAvailable if we're not tracking inventory?