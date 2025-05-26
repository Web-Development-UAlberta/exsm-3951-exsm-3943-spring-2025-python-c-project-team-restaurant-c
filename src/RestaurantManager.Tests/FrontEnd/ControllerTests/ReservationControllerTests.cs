using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using Moq;
using RestaurantManager.Controllers;
using RestaurantManager.Models;
using RestaurantManager.Data;


namespace RestaurantManager.Tests.FrontEnd.ControllerTests{

    public class ReservationControllerTests{

        //Create a Reservation Controller and fake a logged in user (2)
    private ReservationController CreateControllerWithUser(ApplicationDbContext context, string userEmail = "test@example.com", string userId = "2"){

        // Add test user to database
        var testUser = new User{
            Id = int.Parse(userId),
            Email = userEmail,
            FirstName = "Test",
            LastName = "User", 
            Phone = "123-456-7890",
            PasswordHash = "hash",
            PasswordSalt = "salt",
            Role = RestaurantManager.Enums.UserRole.Customer,
            RewardsPoints = 0
        };
        context.Users.Add(testUser);
        context.SaveChanges();

        var controller = new ReservationController(context);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new []{
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userEmail)
        },"test"));

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

        //If a valid Reservation is submitted through Index- the user is redirected to the confirmation page
        //Tests GET
        [Fact]
        public void ReservationRedirectsToConfirmation_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Add mock data to prevent NullReferenceException crash
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
                );


            //Create a Reservation
            var reservation = new Reservation{
                Id = 0, 
                UserId = 2,
                GuestCount = 2,
                TableNumber = 1,
                ReservationDateTime = DateTime.Now.AddHours(1),
                ReservationStatus = Enums.ReservationStatus.Seated,
                CreatedAt = DateTime.UtcNow
            };

            //Pass the reservation to the Index method
            var result = controller.SubmitReservation(reservation) as RedirectToActionResult;

            //Check that the controller returned something
            //Check that the controller redirected to the Confirmation page
            Assert.NotNull(result);
            Assert.Equal("Confirmation", result!.ActionName);
        }  

        //Force an error with the Table Number and check if user is returned to Reservation page
        [Fact]
        public void InvalidReservationReturnsSamePage_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Add mock data to prevent NullReferenceException crash
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            //Force a model error on a field that ISN'T removed by the controller
            controller.ModelState.AddModelError("GuestCount", "Guest count is required");

            //Create a Reservation
            var reservation = new Reservation{
                Id = 0, 
                UserId = 2,
                GuestCount = 2,
                TableNumber = 1,
                ReservationDateTime = DateTime.Now.AddHours(1),
                ReservationStatus = Enums.ReservationStatus.Seated,
                CreatedAt = DateTime.UtcNow
            };

            //Pass the reservation to the Controller method
            var result = controller.SubmitReservation(reservation) as ViewResult;

            //Check that the controller returned something
            //Check that the invalid reservation returns user back to reservation
            Assert.NotNull(result);
            Assert.Equal(reservation, result!.Model);
        }
        
        //If a valid Reservation is submitted through SubmitReservation- the user is redirected to the confirmation page
        //Tests POST
        [Fact]
        public void ValidSubmitReservationReDirectConfirmation_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Add mock data to prevent NullReferenceException crash
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            //Create a Reservation without a TableNumber
            var reservation = new Reservation{
                Id = 0, 
                UserId = 2,
                GuestCount = 2,
                TableNumber = 1,
                ReservationDateTime = DateTime.Now.AddHours(1),
                ReservationStatus = Enums.ReservationStatus.Seated,
                CreatedAt = DateTime.UtcNow
            };

            //Pass the reservation to the Controller method
            var result = controller.SubmitReservation(reservation) as RedirectToActionResult;

            //Check that the controller returned something
            //Check that the user is redirected to Confirmation page
            Assert.NotNull(result);
            Assert.Equal("Confirmation", result!.ActionName);
        } 

        [Fact]
        public void InvalidSubmitReservationReturnsSamePage_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Add mock data to prevent NullReferenceException crash
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            controller.ModelState.AddModelError("ReservationDateTime", "Required");

            //Create a Reservation without a TableNumber
            var reservation = new Reservation{
                Id = 0, 
                UserId = 2,
                GuestCount = 2,
                TableNumber = 1,
                ReservationDateTime = DateTime.Now.AddHours(1),
                ReservationStatus = Enums.ReservationStatus.Seated,
                CreatedAt = DateTime.UtcNow
            };

            //Pass the reservation to the Controller method
            var result = controller.SubmitReservation(reservation) as ViewResult;

            //Check that the controller returned something
            //Check that the user is redirected back to Index
            //Check that we're back on Reservation page
            Assert.NotNull(result);
            Assert.Equal("Index", result!.ViewName);
            Assert.Equal(reservation, result.Model);

        }

        //Tests that Confirmation action returns valid ViewResult
        [Fact]
        public void ConfirmationViewWorks_ShouldPass(){
            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);
            
            //Add mock data to prevent NullReferenceException crash
            controller.TempData = new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>()
            );

            //Force an error
            var result = controller.Confirmation() as ViewResult;

            //Check that a view is returned
            Assert.NotNull(result);
        } 

        //Tests that Error action returns a ErrorViewModel
        [Fact]
        public void ErrorViewWorks_ShouldPass(){
            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

            //Force an error
            var result = controller.Error() as ViewResult;

            //Check that something is returned
            //Check that it contains ErrorViewModel
            Assert.NotNull(result);
            Assert.IsType<ErrorViewModel>(result!.Model);
        }

    }
}