using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

public class SalesRepository(ApplicationDbContext context) : ISalesRepository
{
    public async Task<Sale?> GetByIdAsync(int id)
    {
        return await context
            .Sales.Include(s => s.Customer)
            .Include(s => s.SalesDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<ResponsePage<Sale>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await context.Sales.Where(s => s.IsActive).CountAsync();

        var items = await context
            .Sales.Include(s => s.Customer)
            .Where(s => s.IsActive)
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new ResponsePage<Sale>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
        return response;
    }

    public async Task AddAsync(Sale sale)
    {
        await context.Sales.AddAsync(sale);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        context.Sales.Update(sale);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sale = await context.Sales.FindAsync(id);

        if (sale != null)
        {
            sale.IsActive = false;
            await context.SaveChangesAsync();
        }
    }
}
