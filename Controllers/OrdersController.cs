using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(InventoryDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.Products = _context.Products
                .Where(p => p.UserId == userId).ToList();   // ✅ only user's products
            ViewBag.Suppliers = _context.Suppliers
                .Where(s => s.UserId == userId).ToList();   // ✅ only user's suppliers
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order o)
        {
            ModelState.Remove("UserId");
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
                ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
                return View(o);
            }

            var product = _context.Products
                .FirstOrDefault(p => p.Id == o.ProductId && p.UserId == userId);
            if (product == null)
            {
                ModelState.AddModelError("", "Product not found.");
                ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
                ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
                return View(o);
            }

            if (product.Stock < o.Quantity)
            {
                ModelState.AddModelError("", $"Insufficient stock. Available: {product.Stock}");
                ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
                ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
                return View(o);
            }

            product.Stock -= o.Quantity;
            o.ProductName = product.Name;
            o.OrderDate = DateTime.Now;
            o.UserId = userId;  // ✅ assign user

            _context.Products.Update(product);
            _context.Orders.Add(o);
            _context.SaveChanges();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var order = _context.Orders
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);
            if (order == null) return NotFound();

            ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
            ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order updated)
        {
            ModelState.Remove("UserId");
            var userId = _userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
                ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
                return View(updated);
            }

            var order = _context.Orders
                .FirstOrDefault(o => o.Id == updated.Id && o.UserId == userId);
            if (order == null) return NotFound();

            // restore old stock
            var oldProduct = _context.Products
                .FirstOrDefault(p => p.Id == order.ProductId && p.UserId == userId);
            if (oldProduct != null)
                oldProduct.Stock += order.Quantity;

            // deduct new stock
            var newProduct = _context.Products
                .FirstOrDefault(p => p.Id == updated.ProductId && p.UserId == userId);
            if (newProduct != null)
            {
                if (newProduct.Stock < updated.Quantity)
                {
                    ModelState.AddModelError("", $"Insufficient stock. Available: {newProduct.Stock}");
                    ViewBag.Products = _context.Products.Where(p => p.UserId == userId).ToList();
                    ViewBag.Suppliers = _context.Suppliers.Where(s => s.UserId == userId).ToList();
                    return View(updated);
                }
                newProduct.Stock -= updated.Quantity;
                order.ProductName = newProduct.Name;
                _context.Products.Update(newProduct);
            }

            if (oldProduct != null && oldProduct.Id != updated.ProductId)
                _context.Products.Update(oldProduct);

            order.ProductId    = updated.ProductId;
            order.Quantity     = updated.Quantity;
            order.SupplierName = updated.SupplierName;
            order.Status       = updated.Status;

            _context.Orders.Update(order);
            _context.SaveChanges();

            TempData["Success"] = "Order updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var order = _context.Orders
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);
            if (order != null)
            {
                var product = _context.Products
                    .FirstOrDefault(p => p.Id == order.ProductId && p.UserId == userId);
                if (product != null)
                {
                    product.Stock += order.Quantity;
                    _context.Products.Update(product);
                }
                _context.Orders.Remove(order);
                _context.SaveChanges();
                TempData["Success"] = "Order deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}