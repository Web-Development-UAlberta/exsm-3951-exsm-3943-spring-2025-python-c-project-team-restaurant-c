using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{

    public class PaymentMethod
    {

        [Key]
        public int PaymentMethodId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? PostalCode { get; set; }

        public Customer? Customer { get; set; }
    }
}