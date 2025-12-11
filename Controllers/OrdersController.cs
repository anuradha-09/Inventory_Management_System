using InventoryWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class OrdersController : Controller
{
    private static List<Order> _orders = new List<Order>();
    private static int _nextId = 1;

    // Access products directly from ProductsController
    private static List<Product> _products = ProductsController._products;

    public IActionResult Index() => View(_orders);

    public IActionResult Create()
    {
        ViewBag.Products = _products;
        return View();
    }

    [HttpPost]
    public IActionResult Create(Order o)
    {
        o.Id = _nextId++;
        var prod = _products.FirstOrDefault(p => p.Id == o.ProductId);
        if (prod != null) o.ProductName = prod.Name;
        _orders.Add(o);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var o = _orders.FirstOrDefault(x => x.Id == id);
        ViewBag.Products = _products;
        return View(o);
    }

    [HttpPost]
    public IActionResult Edit(Order updated)
    {
        var o = _orders.FirstOrDefault(x => x.Id == updated.Id);
        if (o != null)
        {
            o.ProductId = updated.ProductId;
            o.Quantity = updated.Quantity;
            o.CustomerName = updated.CustomerName;
            var prod = _products.FirstOrDefault(p => p.Id == updated.ProductId);
            if (prod != null) o.ProductName = prod.Name;
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var o = _orders.FirstOrDefault(x => x.Id == id);
        if (o != null) _orders.Remove(o);
        return RedirectToAction("Index");
    }
}
