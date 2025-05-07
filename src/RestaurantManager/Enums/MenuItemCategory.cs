using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum MenuItemCategory
  {
    [Display(Name = "Appetizers")]
    Appetizer = 0,

    [Display(Name = "Mains")]
    MainCourse = 1,

    [Display(Name = "Desserts")]
    Dessert = 2,

    [Display(Name = "Beverages")]
    Beverage = 3,
  }

}