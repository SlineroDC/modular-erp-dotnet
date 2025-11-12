using ERP.Core.Entities;
using ERP.Core.Models;

namespace ERP.Core.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    
    Task<ResponsePage<Customer>> GetAllAsync(int pageNumber, int pageSize);
    
    Task AddAsync (Customer customer);
    
    Task UpdateAsync (Customer customer);
    
    Task DeleteAsync (int id);

}