using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

public class ProductsController : Controller
{
    private static List<Product> _products = new List<Product>();
    private static int _nextId = 1;

    public IActionResult Index() => View(_products);

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Product p)
    {
        p.Id = _nextId++;
        _products.Add(p);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        return View(p);
    }

    [HttpPost]
    public IActionResult Edit(Product updated)
    {
        var p = _products.FirstOrDefault(x => x.Id == updated.Id);
        if (p != null)
        {
            p.Name = updated.Name;
            p.Stock = updated.Stock;
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        if (p != null) _products.Remove(p);
        return RedirectToAction("Index");
    }
}