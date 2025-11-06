using ERP.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    //Mapping entities to tables
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SalesDetail> SalesDetails { get; set; }
}