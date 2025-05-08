using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models;
using RestaurantManager.Enums;
using Xunit;

namespace RestaurantManager.Tests.BackEnd.ModelValidation{

    public class OrderValidationTest {

        private readonly User testUser = new User{
        Id = 1,
        FirstName = "Ted",
        LastName = "Sheckler",
        Email = "TedShecks@Emporium.com",
        Phone = "780-911-9072",
        PasswordHash = "passwordpassword",
        PasswordSalt = "passwordpassword",
        RewardsPoints = 200,
        Role = UserRole.Customer
        };

        private List<ValidationResult> ValidateModel(Order order){

            var context = new ValidationContext(order);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(order, context, results, true);

            return results;
        }

        //Tests if a negative subtotal can be passed
        [Fact]
        public void OrderSubtotalNegative_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = -1,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Subtotal"));
        }

        //Tests if a too large subtotal can be passed
        [Fact]
        public void OrderSubtotalTooHigh_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 5000000,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Subtotal"));
        }

        //Tests limit of Tax
        [Fact]
        public void OrderTaxTooHigh_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 500,
                Tax = 10001,
                TipAmount = 0,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Tax"));
        }

        //Tests negative Tax
        [Fact]
        public void OrderTaxNegative_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal =7,
                Tax = -1,
                TipAmount = 0,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Tax"));
        }

        //Tests Tip amount above maximum
        [Fact]
        public void OrderTipAmountAboveMaximum_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 9,
                Tax = 1,
                TipAmount = 100001,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("TipAmount"));
        }

        //Tests if a negative Tip Amount can be passed
        [Fact]
        public void OrderTipAmountNegative_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 9,
                Tax = 1,
                TipAmount = -1,
                Total = 1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("TipAmount"));
        }

        //Tests if a negative Total can be passed
        [Fact]
        public void OrderTotalNegative_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 9,
                Tax = 1,
                TipAmount = 0,
                Total = -1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Total"));
        }

        //Tests if a large Total can be passed
        [Fact]
        public void OrderTotalTooLarge_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = -1,
                Tax = 1,
                TipAmount = 0,
                Total = 10000000,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Subtotal"));
        }

        //Tests if a negative DeliveryFee can be passed
        [Fact]
        public void OrderDeliveryFeeNegative_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 9,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                DeliveryFee = -1,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("DeliveryFee"));
        }

        //Tests if a larger than max DeliveryFee can be passed
        [Fact]
        public void OrderDeliveryFeeLargeThanMax_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 9,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                DeliveryFee = 1001,
                Notes = "Bad tipper",
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("DeliveryFee"));
        }

        //Tests a Note past maximum length can be passed
        [Fact]
        public void OrderNoteTooLong_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 4,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                DeliveryFee = 9,
                Notes = new string('B',501),
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("Notes"));
        }

                //Tests a Note past maximum length can be passed
        [Fact]
        public void OrderDeliveryInstructionsTooLong_ShouldFail(){
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 4,
                Tax = 1,
                TipAmount = 0,
                Total = 1,
                DeliveryFee = 9,
                DeliveryInstructions = new string('B',501),
                User = testUser
            };
            
            var results = ValidateModel(order);
            Assert.Contains(results, r => r.MemberNames.Contains("DeliveryInstructions"));
        }

        [Fact]
        public void OrderWithAllValidData_ShouldPass(){

            var order = new Order{
                Id = 10,
                UserId = 10,
                Type = OrderType.Delivery,
                Status = OrderStatus.Completed,
                Subtotal = 200.00M,
                Tax = 50.00M,
                TipAmount = 25.00M,
                Total = 285.00M,
                Notes = "Customer smells like onions",
                DeliveryFee = 10.00M,
                DeliveryInstructions = "Leave at back door. Use passcode: New England Clam Cowder",
                User = testUser
            };

            var results = ValidateModel(order);
            Assert.Empty(results);
        }
    }
}