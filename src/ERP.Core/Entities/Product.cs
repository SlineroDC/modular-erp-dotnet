namespace ERP.Core.Entities;

public class Product
{
    //Propities 
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } 
    public int Stock { get; set; } 
    
    // Internal relations
    public virtual ICollection<SalesDetail> SalesDetails { get; set; } = [];

    // Soft delete
    public bool IsActive { get; set; } = true;
}

