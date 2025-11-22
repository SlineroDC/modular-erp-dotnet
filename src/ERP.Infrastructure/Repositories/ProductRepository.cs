using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

public class ProductRepository (ApplicationDbContext context): IProductRepository
{
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<ResponsePage<Product>> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await context.Products
            .Where(p => p.IsActive)
            .CountAsync();
            

        var items = await context.Products
            .Where(p => p.IsActive)    
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new ResponsePage<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return response;
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product != null)
        {
            product.IsActive = false;

            await context.SaveChangesAsync();
        }
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await context.Products.AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.IsActive);
    }
    
}