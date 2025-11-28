using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
namespace ERP.Admin.Pages;

public class CreateSaleModel(
  ICustomerRepository customerRepository,
  IProductRepository productRepository,
  ISalesRepository salesRepository)
  : PageModel
{
  // The dependence for this page

  public IEnumerable<Product> Products { get; set; } = [];
  public IEnumerable<Customer> Customers { get; set; } = [];

  public async Task OnGetAsync()
  {
    //Get the first 1000 products and customers
    var productsInSales = await productRepository.GetAllAsync(1, 1000);
    var customerInSales = await customerRepository.GetAllAsync(1, 1000);
    
    Products = productsInSales.Items;
    Customers = customerInSales.Items;
    
  }

  //Se
  public async Task<IActionResult> OnPostSaveSaleAsync([FromBody] SaleInputDto input)
  {
    if (input == null || input.Details.Count == 0) return BadRequest("Card is empry");

    var sale = new Sale
    {
      CustomerId = input.CustomerId,
      Total = input.Total,
      Date = DateTime.UtcNow,
      IsActive = true
    };

    foreach (var item in input.Details)
    {
      sale.SalesDetails.Add(new SalesDetail
      {
        ProductId = item.ProductId,
        Quantity = item.Quantity,
        UnitPrice = item.UnitPrice
      });
    }
    await salesRepository.AddAsync(sale);
    return new JsonResult(new { redirectUrl = "/Sales" });
  }
  
  //Dtos for information the JS
  public class SaleInputDto
  {
    public int CustomerId { get; set; }
    public int Total { get; set; }
    public List<SalesDetail> Details { get; set; } = [];
  }
  
  public  class SaleDetailDto
  {
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
  }
}