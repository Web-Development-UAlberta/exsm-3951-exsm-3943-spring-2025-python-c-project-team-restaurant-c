using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum OrderStatus
  {
    [Display(Name = "Pending")]
    Pending = 0,

    [Display(Name = "Completed")]
    Completed = 1,

    [Display(Name = "Cancelled")]
    Cancelled = 2,

    [Display(Name = "In Progress")]
    InProgress = 3,

    [Display(Name = "No Order")]
    NoOrder = 4,
  }

}