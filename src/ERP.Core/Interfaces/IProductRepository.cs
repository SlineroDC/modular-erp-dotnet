using ERP.Core.Entities;
using ERP.Core.Models;

namespace ERP.Core.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<ResponsePage<Product>> GetAllAsync(int pageNumber, int pageSize);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);

    
}