using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum OrderType
  {
    [Display(Name = "DineIn")]
    DineIn,

    [Display(Name = "TakeOut")]
    TakeOut,

    [Display(Name = "Delivery")]
    Delivery
  }
}