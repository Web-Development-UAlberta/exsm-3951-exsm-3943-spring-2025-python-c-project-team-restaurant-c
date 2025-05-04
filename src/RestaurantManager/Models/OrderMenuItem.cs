using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("order_menu_item")]
  public class OrderMenuItem
  {
    [Key, Column(Order = 0)]
    public required int OrderId { get; set; }

    [Key, Column(Order = 1)]
    public required int MenuItemId { get; set; }

    public required int Quantity { get; set; }

    [ForeignKey("OrderId")]
    public required Order Order { get; set; }

    [ForeignKey("MenuItemId")]
    public required MenuItem MenuItem { get; set; }
  }
}