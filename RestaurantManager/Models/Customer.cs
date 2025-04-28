using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Models{

    public class Customer{
        [Key]
        public int CustomerId { get; set; } 

        public string? Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool RewardsActive { get; set; }
        public int RewardsPoints { get; set; }
        public string DietaryNotes { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }
        public List<UserAddress> UserAddresses { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Order> Orders { get; set; }
        public List<UserDietaryTag> UserDietaryTags { get; set; }
    }
}
//Do we need Username? We can hangle logins through email only?
//Do we need rewards to be on/off ever? Is there a scenario where someone wants rewards off?