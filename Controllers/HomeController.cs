using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class HomeController : Controller
{
    private readonly InventoryDbContext _context;

    // Inject DbContext via constructor
    public HomeController(InventoryDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        var orders = _context.Orders.ToList();
        var suppliers = _context.Suppliers.ToList();

        var model = new Home
        {
            TotalProducts = products.Count,
            LowStockProducts = products.Count(p => p.Stock < 5),
            TotalOrders = orders.Count,
            OrdersProcessing = orders.Count(o => o.Status == "Processing"),
            OrdersDelivered = orders.Count(o => o.Status == "Delivered"),
            TotalSuppliers = suppliers.Count
        };

        return View(model);
    }
}
