using InventoryWebApp.Data;
using InventoryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InventoryWebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InventoryDbContext _context;

        public OrdersController(InventoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order o)
        {
            var product = _context.Products.Find(o.ProductId);
            if (product != null)
            {
                o.ProductName = product.Name;
            }

            _context.Orders.Add(o);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order updated)
        {
            var order = _context.Orders.Find(updated.Id);
            if (order != null)
            {
                order.ProductId = updated.ProductId;
                order.Quantity = updated.Quantity;
                order.SupplierName = updated.SupplierName;
                order.Status = updated.Status;

                var product = _context.Products.Find(updated.ProductId);
                if (product != null) order.ProductName = product.Name;

                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
