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
    
    //Relations which SalesDetails
    public ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
}