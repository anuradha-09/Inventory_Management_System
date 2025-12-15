using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryWebApp.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly InventoryDbContext _context;

        public SuppliersController(InventoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Suppliers.ToList());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Supplier s)
        {
            _context.Suppliers.Add(s);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier updated)
        {
            _context.Suppliers.Update(updated);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
