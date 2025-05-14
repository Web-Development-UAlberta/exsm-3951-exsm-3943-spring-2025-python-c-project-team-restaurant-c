using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Xunit;
using Moq;
using RestaurantManager.Controllers;
using RestaurantManager.Models;
using RestaurantManager.Data;
using RestaurantManager.Enums;


namespace RestaurantManager.Tests.FrontEnd.ControllerTests{

    public class OrderControllerTests{
         //Create an Order Controller and fake a UserEmail
        private OrderController CreateControllerWithUser(ApplicationDbContext context, string userEmail = "TScheckler@Emporium.com"){
            
            //Pass controller to the db
            var controller = new OrderController(context);

            //Fake a valid logged in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.Name, userEmail)
            },"test"));

            //Assign said user to the controller context
            controller.ControllerContext = new ControllerContext{
                HttpContext = new DefaultHttpContext{User = user}
            };

            return controller;
        }

        //Create in memory database
        //Fresh db for every test
        private ApplicationDbContext InMemoryDbContext(){

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ApplicationDbContext(options);
        }

        //Check that even if a distance is given - orders tagged as Pickup will have $0 delivery fee
        [Fact]
        public void CheckIfPickUpOrdersHaveNoDeliveryFee_ShouldPass(){
            
            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $50 order with 5km distance (Doesn't matter) and flag it as takeout
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 50.0m, 5.0, OrderType.TakeOut })!;

            //Check that delivery fee is $0
            Assert.Equal(0m, fee);
        }

        //Test if orders with a delivery distance of 5km costs $5.99
        [Fact]
        public void CheckIfDeliveryFeeIsCalculated5km_ShouldPass(){
            
            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $50 order with 5km distance and flag it as delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 50.0m, 5.0, OrderType.Delivery})!;

            //Check that delivery fee is $7.99 for a distance greater than 5km 
            Assert.Equal(5.99m, fee);
        }

        //Test if orders with a delivery distance greater than 5km and less than 8km cost $7.99
        [Fact]
        public void CheckIfDeliveryFeeIsCalculated7km_ShouldPass(){
            
            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $50 order with 7km distance and flag it as Delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 50.0m, 7.0, OrderType.Delivery})!;

            //Check that delivery fee is $7.99 for a distance greater than 5km 
            Assert.Equal(7.99m, fee);
        }

        //Test if orders with a Subtotal of 75 or greater have a free delivery fee
        [Fact]
        public void CheckIfDeliveryFeeIsFreeTotal75_ShouldPass(){
            
            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $75 order, 5km distance (Doesn't matter) and flag it as Delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 75.0m, 7.0, OrderType.Delivery})!;

            //Check that delivery fee is $0 for a Subtotal of $75
            Assert.Equal(0m, fee);
        }

        [Fact]
        public void CheckIfOrderCalculatesTotal_ShouldPass(){

            //Get the CalculateOrderTotal method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateOrderTotal", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Create a user
            var user = new User {
            Id = 1,
            Email = "TSheckler@Emporium.com",
            FirstName = "Ted",
            LastName = "Scheckler",
            Phone = "780-555-2344",
            PasswordHash = "1234",
            PasswordSalt = "1234",
            RewardsPoints = 0,
            Role = UserRole.Customer
            };
            //Add all necessary items for an Order
            var order = new Order{
                Id = 1,
                UserId = 1,
                Type = OrderType.TakeOut,
                Status = OrderStatus.InProgress,
                Subtotal = 20.0m,
                Tax = 1.0m,
                TipAmount = 0.0m,
                Total = 21.0m,
                User = user,
                OrderDate = DateTime.Now,
                OrderMenuItems = new List<OrderMenuItem>{
                    new OrderMenuItem{
                        OrderId = 1,
                        MenuItemId = 1,
                        Quantity = 2,
                        Order = null!,
                        MenuItem = new MenuItem {
                            Id = 1, 
                            Name = "Mozza Sticks", 
                            Description = "Cheesy Goodness", 
                            Price = 10.00m,
                            Category = MenuItemCategory.MainCourse,
                            IsAvailable = true
                        }
                    }
                }
            };

            //Make sure menu item knows which order it belongs to
            order.OrderMenuItems.First().Order = order;
            
            //Add up the total of the 2 items with a $2 tip.
            var total = (decimal)methodInfo!.Invoke(null, new object[] { order, 2.0m, 0.0, OrderType.TakeOut })!;
            
            //Verify that the total includes the correct amount of Tax added ($1.10)
            Assert.Equal(23.10m, total);
        }

        //Tests that Error view returns Error View
        [Fact]
        public void ErrorReturnsErrorViewModel_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Force an error
            var result = controller.Error() as ViewResult;

            //Check that a view is returned
            //Check that result is an Error View Model
            Assert.NotNull(result);
            Assert.IsType<ErrorViewModel>(result!.Model);
        }

    }
}