using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models{

    public class UserAddress{
        [Key]
        public int AddressId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; } 

        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public Customer Customer { get; set; }
    }
}