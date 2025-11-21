using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Admin.Pages;

// Inject both Repository and ExcelService using primary constructor
public class ProductsModel(IProductRepository productRepository, IExcelService excelService) : PageModel
{
    // Data for the view table
    public ResponsePage<Product> Data { get; set; } = new();

    // Property to bind the uploaded Excel file
    [BindProperty] public IFormFile UploadedFile { get; set; }

    // GET: Load paginated products
    public async Task OnGetAsync([FromQuery] int pageNumber = 1)
    {
        const int pageSize = 10;
        Data = await productRepository.GetAllAsync(pageNumber, pageSize);
    }

    // POST: Soft delete a product
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await productRepository.DeleteAsync(id);
        return RedirectToPage();
    }

    // POST: Import products from Excel
    public async Task<IActionResult> OnPostImportAsync()
    {
        if (UploadedFile != null && UploadedFile.Length > 0)
        {
            using var stream = UploadedFile.OpenReadStream();
            await excelService.ImportProductsAsync(stream);
        }

        return RedirectToPage();
    }
}