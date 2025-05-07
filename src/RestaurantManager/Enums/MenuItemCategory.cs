using System.ComponentModel.DataAnnotations;
namespace RestaurantManager.Enums
{
    public enum MenuItemCategory
    {
        [Display(Name = "Appetizers")]
        Appetizer,

        [Display(Name = "Mains")]
        MainCourse,

        [Display(Name = "Desserts")]
        Dessert,

        [Display(Name = "Beverages")]
        Beverage,
    }

}