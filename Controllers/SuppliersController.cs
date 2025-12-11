namespace InventoryWebApp.Controllers
{
    using InventoryWebApp.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class SuppliersController : Controller
    {
        private static List<Supplier> _suppliers = new List<Supplier>();
        private static int _nextId = 1;

        public IActionResult Index() => View(_suppliers);

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Supplier s)
        {
            s.Id = _nextId++;
            _suppliers.Add(s);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var s = _suppliers.FirstOrDefault(x => x.Id == id);
            return View(s);
        }

        [HttpPost]
        public IActionResult Edit(Supplier updated)
        {
            var s = _suppliers.FirstOrDefault(x => x.Id == updated.Id);
            if (s != null)
            {
                s.Name = updated.Name;
                s.Contact = updated.Contact;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var s = _suppliers.FirstOrDefault(x => x.Id == id);
            if (s != null) _suppliers.Remove(s);
            return RedirectToAction("Index");
        }
    }

}
