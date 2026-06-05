using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(InventoryDbContext context,
                              UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var products = _context.Products
                .Where(p => p.UserId == userId).ToList();

            var orders = _context.Orders
                .Where(o => o.UserId == userId).ToList();

            var suppliers = _context.Suppliers
                .Where(s => s.UserId == userId).ToList();

            var model = new Home
            {
                TotalProducts    = products.Count,
                LowStockProducts = products.Count(p => p.Stock < p.LowStockThreshold),
                LowStockList     = products.Where(p => p.Stock < p.LowStockThreshold).ToList(),
                TotalOrders      = orders.Count,
                OrdersProcessing = orders.Count(o => o.Status == "Processing"),
                OrdersDelivered  = orders.Count(o => o.Status == "Delivered"),
                OrdersCancelled  = orders.Count(o => o.Status == "Cancelled"),
                TotalSuppliers   = suppliers.Count,
                RecentOrders     = orders.OrderByDescending(o => o.OrderDate).Take(5).ToList()
            };

            return View(model);
        }
    }
}