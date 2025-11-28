namespace ERP.Core.Entities;

public class SalesDetail
{ 
    //Proprieties 
    public int Id { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } 
    
    //Foreign keys
    public int SalesId { get; set; }
    public int ProductId {get; set;}
    
    public virtual Product? Product { get; set; } 
    public virtual Sale? Sale { get; set; }
}