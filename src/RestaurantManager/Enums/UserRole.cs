using System.ComponentModel.DataAnnotations;

namespace RestaurantManager.Enums
{
  public enum UserRole
  {
    [Display(Name = "Customer")]
    Customer = 0,

    [Display(Name = "Staff")]
    Staff = 1,

    [Display(Name = "Manager")]
    Manager = 2,

    [Display(Name = "Admin")]
    Admin = 3,
  }
}
