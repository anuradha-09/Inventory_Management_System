namespace InventoryWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public string SupplierName { get; set; }  
        public string Status { get; set; }        
    }
}
