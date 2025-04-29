using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models{

    public class UserDietaryTag{

        [Key, Column(Order = 0)]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("DietaryTag")]
        public int TagId {get; set; }

        public Customer Customer { get; set; }
        public DietaryTag DietaryTag { get; set; }
    }
}