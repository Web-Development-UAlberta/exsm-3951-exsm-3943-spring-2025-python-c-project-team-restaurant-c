using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("order_menu_item")]
  public class OrderMenuItem
  {
    [Key, Column(Order = 0)]
    [Required]
    public required int OrderId { get; set; }

    [Key, Column(Order = 1)]
    [Required]
    public required int MenuItemId { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Quantity cannot exceed 1000.")]
    public required int Quantity { get; set; }

    [ForeignKey("OrderId")]
    public required Order Order { get; set; }

    [ForeignKey("MenuItemId")]
    public required MenuItem MenuItem { get; set; }
  }
}