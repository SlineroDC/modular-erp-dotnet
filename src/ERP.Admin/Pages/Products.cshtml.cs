using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Admin.Pages;

public class ProductsModel(IProductRepository productRepository) : PageModel
{
    public ResponsePage<Product> Data { get; set; } = new();

    public async Task OnGetAsync([FromQuery] int pageNumber = 1)
    {
        const int pageSize = 10;

        Data = await productRepository.GetAllAsync(pageNumber, pageSize);
    }
}