using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum OrderType
  {
    [Display(Name = "Dine In")]
    DineIn,

    [Display(Name = "Take Out")]
    TakeOut,

    [Display(Name = "Delivery")]
    Delivery
  }
}