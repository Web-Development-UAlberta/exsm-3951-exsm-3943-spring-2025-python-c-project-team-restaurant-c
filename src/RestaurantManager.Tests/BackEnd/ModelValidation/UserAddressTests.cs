using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models;
using RestaurantManager.Enums;
using Xunit;

namespace RestaurantManager.Tests.BackEnd.ModelValidation{

    public class UserAddressValidationTests {

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

        private List<ValidationResult> ValidateModel(UserAddress address) {
            var context = new ValidationContext(address);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(address, context, results, true);

            return results;
        }

        //Tests the upper limit of AddressLine1
        [Fact]
        public void UserAddressAddress1TooLong_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = new string('A', 101),
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("AddressLine1"));
        }

        //Tests below the minimum of Address Length
        [Fact]
        public void UserAddressAddress1Negative_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AA",
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("AddressLine1"));
        }

        //Tests the over the maximum length of City
        [Fact]
        public void UserAddressCityTooLong_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = new string('B', 51),
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("City"));
        }

        //Tests the under the minimum length of City
        [Fact]
        public void UserAddressCityEmpty_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("City"));
        }

        //Tests the over the max allowed for Province
        [Fact]
        public void UserAddressProvinceTooLong_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = new string ('B', 51),
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("Province"));
        }

        //Tests the under the minimum for Province
        [Fact]
        public void UserAddressProvinceTooShort_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "A",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("Province"));
        }

        //Tests the under the minimum for Country
        [Fact]
        public void UserAddressCountryTooShort_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "C",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("Country"));
        }

        //Tests the under the minimum for Country
        [Fact]
        public void UserAddressCountryTooLong_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = new string ('B', 51),
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("Country"));
        }

        //Tests invalid format for PostalCode
        [Fact]
        public void UserAddressPostalCodeInvalid_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                City = "Edmonton",
                PostalCode = "12345",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("PostalCode"));
        }

        //Tests over the maximum for AddressLine2
        [Fact]
        public void UserAddressAddressLine2TooLong_ShouldFail() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                AddressLine2 = new string('B', 101),
                City = "Edmonton",
                PostalCode = "T3F8HJ9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Contains(results, r => r.MemberNames.Contains("AddressLine2"));
        }

        //Tests all valid input types
        [Fact]
        public void UserAddressAllValidInput_ShouldPass() {

            var userAddress = new UserAddress{
                Id = 1,
                UserId = 1,
                AddressLine1 = "AAA",
                AddressLine2 = "123 Fake Street",
                City = "Edmonton",
                PostalCode = "T3F8H9",
                Province = "Alberta",
                Country = "Canada",
                User = testUser
            };

            var results = ValidateModel(userAddress);
            Assert.Empty(results);
        }

    }
}