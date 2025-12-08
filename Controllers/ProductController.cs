using Inventory_Management_System.Models;
using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace Inventory_Management_System.Controllers
{
    public class ProductController : Controller
    {
        private readonly InventoryDbContext _context;

        public ProductController(InventoryDbContext context)
        {
            _context = context;
        }

        // ----------------------------------------------------------
        // GET: /Product
        // ----------------------------------------------------------
        public IActionResult Index(string q)
        {
            var products = _context.Products
                                   .OrderBy(p => p.Name)
                                   .ToList();

            if (!string.IsNullOrEmpty(q))
            {
                products = products
                    .Where(p => p.Name.ToLower().Contains(q.ToLower()))
                    .ToList();
            }

            return View(products);
        }

        // ----------------------------------------------------------
        // GET: /Product/Create
        // ----------------------------------------------------------
        public IActionResult Create()
        {
            ViewBag.Suppliers = _context.Suppliers.ToList();
            return View();
        }

        // ----------------------------------------------------------
        // POST: /Product/Create
        // ----------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // IMPORTANT: repopulate dropdown when returning view
            ViewBag.Suppliers = _context.Suppliers.ToList();

            return View(product);
        }

        // ----------------------------------------------------------
        // GET: /Product/Edit/5
        // ----------------------------------------------------------
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            ViewBag.Suppliers = _context.Suppliers.ToList();

            return View(product);
        }

        // ----------------------------------------------------------
        // POST: /Product/Edit/5
        // ----------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Suppliers = _context.Suppliers.ToList();

            return View(product);
        }

        // ----------------------------------------------------------
        // GET: /Product/Delete/5
        // ----------------------------------------------------------
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                                  .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // ----------------------------------------------------------
        // POST: /Product/DeleteConfirmed/5
        // ----------------------------------------------------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // ----------------------------------------------------------
        // GET: /Product/Details/5
        // ----------------------------------------------------------
        public IActionResult Details(int id)
        {
            var product = _context.Products
                                  .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
