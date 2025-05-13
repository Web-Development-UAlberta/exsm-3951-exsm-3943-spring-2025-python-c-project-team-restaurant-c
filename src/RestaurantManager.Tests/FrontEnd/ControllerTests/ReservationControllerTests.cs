using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Xunit;
using RestaurantManager.Controllers;
using RestaurantManager.Models;
using RestaurantManager.Data;


namespace RestaurantManager.Tests.FrontEnd.ControllerTests{

    public class ReservationControllerTests{

        //Create a Reservation Controller and fake a logged in user (2)
        private ReservationController CreateControllerWithUser(ApplicationDbContext context, string userId = "2"){

            //Pass controller to the db
            var controller = new ReservationController(context);

            //Fake a valid logged in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new []{
                new Claim(ClaimTypes.NameIdentifier, userId)
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

        //If a valid Reservation is submitted - the user is redirected to the confirmation page
        [Fact]
        public void ReservationRedirectsToConfirmation_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateControllerWithUser(context);

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
            var result = controller.Index(reservation) as RedirectToActionResult;

            //Check that the controller returned something
            //Check that the controller redirected to the Confirmation page
            Assert.NotNull(result);
            Assert.Equal("Confirmation", result!.ActionName);
        }
    }
}