using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await context.Customers.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<ResponsePage<Customer>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null
    )
    {
        var query = context.Customers.Where(c => c.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();

            query = query.Where(c =>
                c.Name.ToLower().Contains(term) || c.Email.ToLower().Contains(term)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(c => c.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new ResponsePage<Customer>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
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
        var existing = await context.Customers.FindAsync(customer.Id);
        if (existing != null)
        {
            existing.Name = customer.Name;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;
            existing.IdDocument = customer.IdDocument;
            existing.Phone = customer.Phone;
            existing.Address = customer.Address;
        }

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

    public async Task<bool> IsDuplicateAsync(string email, string document, int? excludeId = null)
    {
        var query = context.Customers.AsQueryable();

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync(c =>
            (c.Email == email || c.IdDocument == document) && c.IsActive
        );
    }
}
