using ERP.Core.Entities;
using ERP.Core.Models;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Customer?> GetByIsAsync(int id)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<ResponsePage<Customer>> GetAllAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Customers
            .Where(c => c.IsActive)
            .CountAsync();
            

        var items = await _context.Customers
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
        await _context.Customers.AddAsync(customer);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer != null)
        {
            customer.IsActive = false;

            await _context.AddRangeAsync();
        }
    }
}