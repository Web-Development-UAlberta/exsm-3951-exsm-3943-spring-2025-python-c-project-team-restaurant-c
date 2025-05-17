using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums;

public enum TakeOutOptions
{
    [Display(Name = "Standard - Ready In 30 Minutes")]
    Standard = 0,

    [Display(Name = "Book a Pickup Time")]
    BookATime = 1,
}