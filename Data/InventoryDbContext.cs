using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Inventory_Management_System.Models;
public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}
