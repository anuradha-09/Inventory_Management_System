using System;
namespace Inventory_Management_System.Models;
public class Order
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime OrderDate { get; set; }
}
