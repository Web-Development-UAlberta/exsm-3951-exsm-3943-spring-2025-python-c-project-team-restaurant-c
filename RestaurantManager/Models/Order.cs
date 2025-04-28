using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestaurantManager.Models{
    
    public class Order{

        [Key]
        public int OrderId { get; set; }
        
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Reservation")]
        public int ReservationId { get; set; }

        [ForeignKey("UserAddress")]
        public int? AddressId { get; set; }

        public string OrderType { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public decimal? TipAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Notes { get; set; }
        public decimal? DeliveryFee { get; set; }
        public string DeliveryInstructions { get; set; }
        public DateTime? ScheduledTime { get; set; }

        public Customer Customer { get; set; }
        public Reservation Reservation { get; set; }
        public UserAddress Address { get; set; }
        public List<OrderMenuItem> OrderMenuItems { get; set; }
    }
}