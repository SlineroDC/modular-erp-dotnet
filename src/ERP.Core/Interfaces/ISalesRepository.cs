using ERP.Core.Entities;
using ERP.Core.Models;

namespace ERP.Core.Interfaces;

public interface ISalesRepository
{
    Task<Sale?> GetByIdAsync(int id);
    Task<ResponsePage<Sale>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task AddAsync(Sale venta);
    
    Task UpdateAsync(Sale venta);
    Task DeleteAsync(int id);
}