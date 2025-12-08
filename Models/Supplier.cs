using System.Collections.Generic;
namespace Inventory_Management_System.Models;
public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Product> Products { get; set; }
}
