using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<ResponsePage<Customer>> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await context.Customers
            .Where(c => c.IsActive)
            .CountAsync();


        var items = await context.Customers
            .Where(c => c.IsActive)
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new ResponsePage<Customer>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        return response;
    }

    public async Task AddAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await context.Customers.FindAsync(id);

        if (customer != null)
        {
            customer.IsActive = false;

            await context.SaveChangesAsync();
        }
    }
}