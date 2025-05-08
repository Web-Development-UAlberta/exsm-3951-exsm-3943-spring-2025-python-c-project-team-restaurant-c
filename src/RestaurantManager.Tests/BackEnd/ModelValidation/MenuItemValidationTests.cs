using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models;
using RestaurantManager.Enums;
using Xunit;

namespace RestaurantManager.Tests.BackEnd.ModelValidation{


    public class MenuItemValidationTest{
        
        //Creates a MenuItem and checks if it passes validation rules
        private List<ValidationResult> ValidateModel(MenuItem menuItem){

            var context = new ValidationContext(menuItem);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(menuItem, context, results, true);

            return results;
        }

        //Testing a valid menu Item - Should pass
        [Fact]
        public void ValidMenuItem_ShouldPass(){

            var menuItem = new MenuItem{
                Id = 1,
                Name = "Yam Fries",
                Description = "Potatoes? No! Yams? Yes! Enjoy These yam fries!",
                Price = 9.99M,
                Category = MenuItemCategory.MainCourse,
                IsAvailable = true
            };

            var results = ValidateModel(menuItem);
            Assert.Empty(results);
        }

        //Creates a menu iutem with a too long Name and checks if validation fails for Name
        [Fact]
        public void MenuItemNameTooLong_ShouldFail(){

            var menuItem = new MenuItem{
                Id = 2,
                Name = new string('B',101),
                Description = "Beer with very long name",
                Price = 5.99M,
                Category = MenuItemCategory.Beverage,
                IsAvailable = true
            };
            
            //Check if the error is in the Name property
            var results = ValidateModel(menuItem);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void MenuItemMissingName_ShouldFail(){

            var menuItem = new MenuItem{
                Id = 3,
                Name = null!,
                Description = "Missing Item",
                Price = 4.99M,
                Category = MenuItemCategory.MainCourse,
                IsAvailable = true
            };

            //Check if the error is in the Name property
            var results = ValidateModel(menuItem);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }
        //Can we test a category enum that's missing if it's required? Leaving here for now.
       /* [Fact]
        public void MenuItemMissingCategory_ShouldFail(){
            var menuItem = new MenuItem{
                Id = 4,
                Name = "Breadsticks",
                Description = "Garlic Breadsticks",
                Price = 4.99M,
                Category = "",
                IsAvailable = true
            };

            //Check if the error is in the Category property
            var results = ValidateModel(menuItem);
            Assert.Contains(results, r => r.MemberNames.Contains("Category")); 
        } */

        //Checks if a negative price can be passed
        [Fact]
        public void MenuItemPriceNegative_ShouldFail(){
            var menuItem = new MenuItem{
                Id = 4,
                Name = "Goodbye Fry",
                Description = "Where is it?",
                Price = -0.1M,
                Category = MenuItemCategory.Appetizer,
                IsAvailable = true
            };

            //Check if the error is in the Price property
            var results = ValidateModel(menuItem);
            Assert.Contains(results, r => r.MemberNames.Contains("Price"));
        }

        //Checks the upper limit of a menu item's price
        [Fact]
        public void MenuItemPriceTooHigh_ShouldFail() {
            var menuItem = new MenuItem{
                Id = 5,
                Name = "Expensive Cheese",
                Description = "The Best Ever!",
                Price = 20001.0M,
                Category = MenuItemCategory.Appetizer,
                IsAvailable = true,
            };

            //Check if the error is in the Price property
            var results = ValidateModel(menuItem);
            Assert.Contains(results, r => r.MemberNames.Contains("Price"));
        }

    }
}