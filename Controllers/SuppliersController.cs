using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryWebApp.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly InventoryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuppliersController(InventoryDbContext context,
                                   UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            return View(_context.Suppliers
                .Where(s => s.UserId == userId).ToList());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier s)
        {
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
                return View(s);

            s.UserId = _userManager.GetUserId(User);  // ✅ assign user
            _context.Suppliers.Add(s);
            _context.SaveChanges();
            TempData["Success"] = "Supplier added successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.Id == id && s.UserId == userId);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier updated)
        {
            ModelState.Remove("UserId");
            if (!ModelState.IsValid)
                return View(updated);

            var userId = _userManager.GetUserId(User);
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.Id == updated.Id && s.UserId == userId);
            if (supplier == null) return NotFound();

            supplier.Name    = updated.Name;
            supplier.Contact = updated.Contact;
            supplier.Email   = updated.Email;
            supplier.Address = updated.Address;

            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
            TempData["Success"] = "Supplier updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var supplier = _context.Suppliers
                .FirstOrDefault(s => s.Id == id && s.UserId == userId);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                TempData["Success"] = "Supplier deleted successfully!";
            }
            return RedirectToAction("Index");
        }
    }
}