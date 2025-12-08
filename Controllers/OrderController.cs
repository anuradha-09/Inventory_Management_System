using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory_Management_System.Models;

public class OrderController : Controller
{
    private readonly InventoryDbContext _context;

    public OrderController(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders.Include(o => o.Product).ToListAsync();
        return View(orders);
    }

    public IActionResult Create()
    {
        ViewBag.Products = _context.Products.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Products = _context.Products.ToList();
        return View(order);
    }
}
