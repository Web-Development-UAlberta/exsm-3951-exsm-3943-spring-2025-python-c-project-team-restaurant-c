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

    [Display(Name = "InProgress")]
    InProgress = 3,
  }

}