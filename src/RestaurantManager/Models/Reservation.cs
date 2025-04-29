using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace RestaurantManager.Models{
    public class Reservation{
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public DateTime ReservationDateTime { get; set; }
        public int GuestCount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public int? TableNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Customer Customer { get; set; }
        public Order Order { get; set; }
    }
}