using Inventory_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class SupplierController : Controller
{
    private readonly InventoryDbContext _context;

    public SupplierController(InventoryDbContext context)
    {
        _context = context;
    }

    // ----------------------------------------------------------
    // GET: /Supplier
    // ----------------------------------------------------------

    public async Task<IActionResult> Index()
    {
        var suppliers = await _context.Suppliers.Include(s => s.Products).ToListAsync();
        return View(suppliers);
    }


    // ----------------------------------------------------------
    // GET: /Supplier/Create
    // ----------------------------------------------------------
    public IActionResult Create()
    {
        return View();
    }

    // ----------------------------------------------------------
    // POST: /Supplier/Create
    // ---------------------------------------------------------


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        Console.WriteLine("POST Create called");

        if (ModelState.IsValid)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            Console.WriteLine("Inserted supplier");
            Console.WriteLine("DB = " + _context.Database.GetDbConnection().Database);

            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("Model invalid");

        return View(supplier);
    }




    // ----------------------------------------------------------
    // GET: /Supplier/Edit/5
    // ----------------------------------------------------------
    public IActionResult Edit(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        if (supplier == null)
            return NotFound();

        return View(supplier);
    }

    // ----------------------------------------------------------
    // POST: /Supplier/Edit/5
    // ----------------------------------------------------------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Supplier supplier)
    {
        if (id != supplier.SupplierId)
            return BadRequest();

        if (ModelState.IsValid)
        {
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(supplier);
    }

    // ----------------------------------------------------------
    // GET: /Supplier/Delete/5
    // ----------------------------------------------------------
    public IActionResult Delete(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        if (supplier == null)
            return NotFound();

        return View(supplier);
    }

    // ----------------------------------------------------------
    // POST: /Supplier/DeleteConfirmed
    // ----------------------------------------------------------
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var supplier = _context.Suppliers.Find(id);

        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
