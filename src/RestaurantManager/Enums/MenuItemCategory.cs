using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum MenuItemCategory
  {
    [Display(Name = "Appetizer")]
    Appetizer = 0,

    [Display(Name = "MainCourse")]
    MainCourse = 1,

    [Display(Name = "Dessert")]
    Dessert = 2,

    [Display(Name = "Beverage")]
    Beverage = 3,
  }

}