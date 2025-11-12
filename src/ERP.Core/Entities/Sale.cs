using System.Collections;

namespace ERP.Core.Entities;

public sealed class Sale
{
    //Properties
    public int Id { get; set; } 
    public DateTime Date { get; set; } 
    public decimal Total {get; set;} 

    //Foreign key
    public int CustomerId { get; set; } 
    public required Customer Customer { get; set; }
    
    //Soft delete
    public bool IsActive { get; set; } = true;
    //Relations which ISalesDetailsRepository
    public ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}