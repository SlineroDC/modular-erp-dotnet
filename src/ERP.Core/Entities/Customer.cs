
namespace ERP.Core.Entities;

public class Customer
{
    public Customer()
    {
        
    }
    //Proprieties
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdDocument { get; set; } = string.Empty;
    public string? Phone { get; set; } 
    public string Address { get; set; } 
    
    // virtual collection
    public virtual ICollection<Sale> Sales { get; set; } = [];
    
    // Disable the Customer
    public bool IsActive { get; set; } = true;
}