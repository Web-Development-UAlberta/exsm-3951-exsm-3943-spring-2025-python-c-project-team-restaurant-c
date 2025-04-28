using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models{

    public class OrderMenuItem{
        [Key, Column(Order = 0)]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        public int Quantity { get; set; }
        
        public Order Order { get; set; }
        public MenuItem MenuItem{ get; set; }
    }
}