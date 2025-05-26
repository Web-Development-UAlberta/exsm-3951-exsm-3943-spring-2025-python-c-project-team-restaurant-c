using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using RestaurantManager.Controllers;
using RestaurantManager.Models;
using RestaurantManager.Data;
using RestaurantManager.Enums;


namespace RestaurantManager.Tests.FrontEnd.ControllerTests{

    public class OrderControllerTests
    {
        //Create an Order Controller and fake a UserEmail
        private OrderController CreateControllerWithUser(ApplicationDbContext context, string userEmail = "TScheckler@Emporium.com")
        {

            //Pass controller to the db
            var controller = new OrderController(context);

            //Fake a valid logged in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.Name, userEmail)
            }, "test"));

            //Assign said user to the controller context
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        //Create in memory database
        //Fresh db for every test
        private ApplicationDbContext InMemoryDbContext()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        //Check that even if a distance is given - orders tagged as Pickup will have $0 delivery fee
        [Fact]
        public void CheckIfPickUpOrdersHaveNoDeliveryFee_ShouldPass()
        {

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
        public void CheckIfDeliveryFeeIsCalculated5km_ShouldPass()
        {

            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $50 order with 5km distance and flag it as delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 50.0m, 5.0, OrderType.Delivery })!;

            //Check that delivery fee is $7.99 for a distance greater than 5km 
            Assert.Equal(5.99m, fee);
        }

        //Test if orders with a delivery distance greater than 5km and less than 8km cost $7.99
        [Fact]
        public void CheckIfDeliveryFeeIsCalculated7km_ShouldPass()
        {

            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $50 order with 7km distance and flag it as Delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 50.0m, 7.0, OrderType.Delivery })!;

            //Check that delivery fee is $7.99 for a distance greater than 5km 
            Assert.Equal(7.99m, fee);
        }

        //Test if orders with a Subtotal of 75 or greater have a free delivery fee
        [Fact]
        public void CheckIfDeliveryFeeIsFreeTotal75_ShouldPass()
        {

            //Get the CalculateDeliveryFee method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateDeliveryFee",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Add a $75 order, 5km distance (Doesn't matter) and flag it as Delivery
            var fee = (decimal)methodInfo!.Invoke(null, new object[] { 75.0m, 7.0, OrderType.Delivery })!;

            //Check that delivery fee is $0 for a Subtotal of $75
            Assert.Equal(0m, fee);
        }

        //Tests to see if Subtotal is correctly calculated
        [Fact]
        public void CheckIfSubtotalCalculates_ShouldPass()
        {

            //Get the CalculateSubtotal method
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateSubtotal",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(methodInfo);

            //2 items at $10.00 each
            var itemPrices = new List<(int, decimal)> { (2, 10.00m) };

            var subtotal = (decimal)methodInfo.Invoke(null, new object[] { itemPrices })!;

            //Check if subtotal is $20.00
            Assert.Equal(20.00m, subtotal);
        }

        //Tests that Error view returns Error View
        [Fact]
        public void ErrorReturnsErrorViewModel_ShouldPass()
        {

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

        //Check to see if tip is correctly calculated
        [Fact]
        public void CheckIfTipCalculatesCorrectly_ShouldPass()
        {

            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateTip",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(methodInfo);

            //Test custom tip amount
            var tip = (decimal)methodInfo.Invoke(null, new object[] { "0", 5.50m })!;
            Assert.Equal(5.50m, tip);
        }

        //Test to see if Tax is caluclated correctly
        [Fact]
        public void CheckIfTaxCalculatesCorrectly_ShouldPass()
        {
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateTax",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(methodInfo);

            //(Subtotal and Delivery)$25 * 5% = $1.25 tax
            var tax = (decimal)methodInfo.Invoke(null, new object[] { 20.00m, 5.00m })!;
            Assert.Equal(1.25m, tax);
        }
        
        [Fact]
        public void CheckIfRewardsCalculateCorrectly_ShouldPass(){
            var methodInfo = typeof(OrderController).GetMethod(
                "CalculateRewardDiscount", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            
            Assert.NotNull(methodInfo);
            
            //Add enough points for discount
            object[] parameters = { true, 300, 0 };
            var discount = (decimal)methodInfo.Invoke(null, parameters)!;

            //Check if appropriate discount added
            //Check if reward points are deducted 
            Assert.Equal(-20.00m, discount);
            Assert.Equal(250, parameters[2]); 
        }        

    }
}