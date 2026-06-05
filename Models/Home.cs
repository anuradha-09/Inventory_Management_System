namespace InventoryWebApp.Models
{
    public class Home
    {
        public int TotalProducts { get; set; }
        public int LowStockProducts { get; set; }
        public List<Product> LowStockList { get; set; } = new();
        public int TotalOrders { get; set; }
        public int OrdersProcessing { get; set; }
        public int OrdersDelivered { get; set; }
        public int OrdersCancelled { get; set; }
        public int TotalSuppliers { get; set; }
        public List<Order> RecentOrders { get; set; } = new();
    }
}