using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManager.Models
{
  [Table("order_menu_item")]
  [PrimaryKey(nameof(OrderId), nameof(MenuItemId))]
  public class OrderMenuItem
  {
    [Column("order_id", TypeName = "INTEGER"), Required]
    public required int OrderId { get; set; }

    [Column("menu_item_id", TypeName = "INTEGER"), Required]
    public required int MenuItemId { get; set; }

    [Column("quantity", TypeName = "INTEGER"), Required]
    public required int Quantity { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [ForeignKey("MenuItemId")]
    public MenuItem? MenuItem { get; set; }
  }
}