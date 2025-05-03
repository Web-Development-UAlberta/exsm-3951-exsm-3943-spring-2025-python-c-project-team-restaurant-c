using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManager.Models
{
  [Table("order_menu_item")]
  public class OrderMenuItem
  {
    [Key, Column(Order = 0)]
    public int OrderId { get; set; }

    [Key, Column(Order = 1)]
    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [ForeignKey("MenuItemId")]
    public MenuItem? MenuItem { get; set; }
  }
}