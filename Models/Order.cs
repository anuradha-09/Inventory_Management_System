using System.ComponentModel.DataAnnotations;

namespace InventoryWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string? SupplierName { get; set; }
        public string Status { get; set; } = "Processing";
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string UserId { get; set; } 
    }
}