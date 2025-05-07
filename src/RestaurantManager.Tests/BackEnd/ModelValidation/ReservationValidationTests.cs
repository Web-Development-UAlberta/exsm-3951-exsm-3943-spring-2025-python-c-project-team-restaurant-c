using System.ComponentModel.DataAnnotations;
using RestaurantManager.Models;
using RestaurantManager.Enums;
using Xunit;

namespace RestaurantManager.Tests.BackEnd.ModelValidation {

    public class ReservationValidationTests {
        private List<ValidationResult> ValidateModel(Reservation reservation){
            var context = new ValidationContext(reservation);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(reservation, context, results, true);
            
            return results;
        }

        //Tests if we can reserve with a guest count more than 50 people
        [Fact]
        public void ReservationGuestCountPastMax_ShouldFail() {
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 51,
                Status = OrderStatus.Pending,
                TableNumber= 4,
                CreatedAt = DateTime.Now
            };

            //Check if the error is in the GuestCount property
            var results = ValidateModel(reservation);
            Assert.Contains(results, r => r.MemberNames.Contains("GuestCount"));            
        }

        //Tests if we can make a reservation with guest count of 0
        [Fact]
        public void ReservationGuestCountBelowMin_ShouldFail(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 0,
                Status = OrderStatus.Pending,
                TableNumber= 4,
                CreatedAt = DateTime.Now
            };

            //Check if the error is in the GuestCount property
            var results = ValidateModel(reservation);
            Assert.Contains(results, r => r.MemberNames.Contains("GuestCount"));       
        }

        //Test with a valid guest count
        [Fact]
        public void ReservationGuestCountValid_ShouldPass(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Status = OrderStatus.Pending,
                TableNumber= 7,
                CreatedAt = DateTime.Now
            };

            //Check if no errors are thrown
            var results = ValidateModel(reservation);
            Assert.Empty(results);     
        }

        [Fact]
        public void ReservationTableNumberTooLow_ShouldFail(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Status = OrderStatus.Pending,
                TableNumber= 0,
                CreatedAt = DateTime.Now
            };

            //Check if the error is in the TableNumber property
            var results = ValidateModel(reservation);
            Assert.Contains(results, r => r.MemberNames.Contains("TableNumber"));       
        }

        [Fact]
        public void ReservationTableNumberTooLHigh_ShouldFail(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Status = OrderStatus.Pending,
                TableNumber= 31,
                CreatedAt = DateTime.Now
            };

            //Check if the error is in the TableNumber property
            var results = ValidateModel(reservation);
            Assert.Contains(results, r => r.MemberNames.Contains("TableNumber"));       
        }

        //Check with a valid table number
        [Fact]
        public void ReservationTableNumberValid_ShouldPass(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Notes = "",
                Status = OrderStatus.Pending,
                TableNumber= 7,
                CreatedAt = DateTime.Now
            };

            //Check if no errors are thrown
            var results = ValidateModel(reservation);
            Assert.Empty(results);     
        }

        //Test to see if we can go past the notes limit
        [Fact]
        public void ReservationTableNotesTooLow_ShouldFail(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Notes = new string('B', 501),
                Status = OrderStatus.Pending,
                TableNumber= 0,
                CreatedAt = DateTime.Now
            };

            //Check if the error is in the notes property
            var results = ValidateModel(reservation);
            Assert.Contains(results, r => r.MemberNames.Contains("Notes"));       
        }

        //Test to see if we can pass an empty note
        [Fact]
        public void ReservationTableNotesEmptyString_ShouldPass(){
            var reservation = new Reservation{
                Id = 1,
                UserId = 1,
                ReservationDateTime = DateTime.Now.AddDays(2),
                GuestCount = 6,
                Notes = "",
                Status = OrderStatus.Pending,
                TableNumber= 7,
                CreatedAt = DateTime.Now
            };

            //Check if no errors are thrown
            var results = ValidateModel(reservation);
            Assert.Empty(results);     
        }

    }
}