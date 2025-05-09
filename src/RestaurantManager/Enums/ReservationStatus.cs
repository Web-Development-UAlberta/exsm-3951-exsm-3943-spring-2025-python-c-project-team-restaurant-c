using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
    public enum ReservationStatus
    {
        [Display(Name = "Booked")]
        Booked = 0,

        [Display(Name = "Seated")]
        Seated = 1,

        [Display(Name = "Completed")]
        Completed = 2,

        [Display(Name = "Cancelled")]
        Cancelled = 3,
    }

}