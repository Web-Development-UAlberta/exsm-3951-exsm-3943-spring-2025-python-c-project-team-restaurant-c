using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RestaurantManager.Controllers;
using RestaurantManager.Data;
using RestaurantManager.Models;
using System.Security.Claims;
using Xunit;

namespace RestaurantManager.Tests.FrontEnd.ControllerTests{

    public class AccountControllerTests{

        private AccountController CreateController(ApplicationDbContext context, string userEmail = "TScheckler@Emporium.com"){

            var login = new Mock<ILogger<AccountController>>().Object;
            var controller = new AccountController(login, context);

            //Fake a valid logged in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]{
                new Claim(ClaimTypes.Name, userEmail),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Customer") 
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

        //Tests that hashing the same password will result in the same output
        [Fact]
        public void ReturnsSameHashForSameInput_ShouldPass(){

            //Get the HashPassword method
            var methodInfo = typeof(AccountController).GetMethod(
                "HashPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Dummy data
            var pass = "ThisIsATestPassword12!&";
            var salt = "ThisIsTestSalt12!&";

            //Hash the same data twice
            var firstHash = (string)methodInfo!.Invoke(null, new object[] {pass, salt})!;
            var secondHash = (string)methodInfo!.Invoke(null, new object[] {pass, salt})!;

            //Check if both hashes are the same
            Assert.Equal(firstHash, secondHash);           
        }

        //Tests that a newly Hashed password is verified
        [Fact]
        public void CheckVerifyPasswordWithValidPassword_ShouldPass(){

            //Get the HashPassword method
            var methodInfoHash = typeof(AccountController).GetMethod(
                "HashPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Get the HashPassword method
            var methodInfoVerify = typeof(AccountController).GetMethod(
                "VerifyPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            //Dummy data
            var pass = "ThisIsATestPassword12!&";
            var salt = "ThisIsTestSalt12!&";

            //Hash the password
            var firstHash = (string)methodInfoHash!.Invoke(null, new object[] {pass, salt})!;

            //Attempt to verify the password
            var result = (bool)methodInfoVerify!.Invoke(null, new object[] {pass, firstHash, salt})!;

            //Check if verification passes
            Assert.True(result);
        }

        //Test that when a customer visits the login page the correct view and data is returned
        //Mimics initial page load
        [Fact]
        public void LoginReturnsLoginView_ShouldPass(){
            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateController(context);

            //Try to view Login as a customer (0)
            var result = controller.Login(0) as ViewResult;

            //Check that a view is returned
            //Check that it's showing the login page
            //Make sure it knows that we're a customer
            //Confirm page knows we're on the login screen
            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName);
            Assert.Equal(0, result.ViewData["UserType"]);
            Assert.True((bool)result.ViewData["IsLogin"]!);
        }

        //Tests that the Register page loads correctly
        [Fact]
        public void RegisterReturnsRegisterView_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateController(context);

            //Try to view Register Page
            var result = controller.Register() as ViewResult;

            //Check that a view is returned
            //Check that it's showing the Register Page
            //Confirm we're not on Login page
            Assert.NotNull(result);
            Assert.Equal("Register", result.ViewName);
            Assert.False((bool)result.ViewData["IsLogin"]!);
        }

        //Tests that Error view returns Error View
        [Fact]
        public void ErrorReturnsErrorViewModel_ShouldPass(){

            //Set up db and controller with user
            var context = InMemoryDbContext();
            var controller = CreateController(context);

            //Force an error
            var result = controller.Error() as ViewResult;

            //Check that a view is returned
            //Check that result is an Error View Model
            Assert.NotNull(result);
            Assert.IsType<ErrorViewModel>(result!.Model);
        }
    }


}