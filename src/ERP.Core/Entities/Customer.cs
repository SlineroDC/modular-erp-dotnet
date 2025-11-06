
namespace ERP.Core.Entities;

public class Customer
{
    public Customer()
    {
        
    }
    //Propities 
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdDocument { get; set; } = string.Empty;
    public string? Phone { get; set; } 
    public string? Address { get; set; } 
    
    // vitural collection
    public virtual ICollection<Sale> Sales { get; set; } = [];
    
    // Desactive the Customer
    public bool IsActive { get; set; } = true;
}