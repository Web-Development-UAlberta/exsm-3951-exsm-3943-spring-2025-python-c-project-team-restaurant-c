using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models;
using RestaurantManager.Enums;
using Xunit;

namespace RestaurantManager.Tests.BackEnd.ModelValidation{

    public class DietaryTagValidationTests{

        private List<ValidationResult> ValidateModel(DietaryTag tag){

            var context = new ValidationContext(tag);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(tag, context, results, true);

            return results;
        }

        //Testing for a missing Name
        [Fact]
        public void DietaryTagNameMissing_ShouldFail(){
            var tag = new DietaryTag{
                Id = 1,
                Name = null!
            };

            var results = ValidateModel(tag);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        //Testing past the maximum allowed for Name
        [Fact]
        public void DietaryTagNameTooLong_ShouldFail(){
            var tag = new DietaryTag{
                Id = 1,
                Name = new string('B', 51)
            };

            var results = ValidateModel(tag);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        //Testing for a Valid Name
        [Fact]
        public void DietaryTagNameValid_ShouldFail(){
            var tag = new DietaryTag{
                Id = 1,
                Name = "Vegan"
            };

            var results = ValidateModel(tag);
            Assert.Empty(results);
        }

        //Testing for a Name right at Maximum
        [Fact]
        public void DietaryTagMissing_ShouldFail(){
            var tag = new DietaryTag{
                Id = 1,
                Name = new string('B', 50)
            };

            var results = ValidateModel(tag);
            Assert.Empty(results);
        }
    }
}