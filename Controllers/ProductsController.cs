using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(InventoryDbContext context,
                                  UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var products = _context.Products
                .Where(p => p.UserId == userId)
                .ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ModelState.Clear();  
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product p)
        {

            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
                return View(p);

            p.UserId = _userManager.GetUserId(User);  
            _context.Products.Add(p);
            _context.SaveChanges();
            TempData["Success"] = "Product added successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product updated)
        {
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
                return View(updated);

            var userId = _userManager.GetUserId(User);
            var product = _context.Products
                .FirstOrDefault(p => p.Id == updated.Id && p.UserId == userId);
            if (product == null) return NotFound();

            product.Name = updated.Name;
            product.Category = updated.Category;
            product.Price = updated.Price;
            product.Stock = updated.Stock;
            product.LowStockThreshold = updated.LowStockThreshold;

            _context.Products.Update(product);
            _context.SaveChanges();
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["Success"] = "Product deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}