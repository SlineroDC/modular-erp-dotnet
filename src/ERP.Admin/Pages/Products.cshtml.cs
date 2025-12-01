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
        // Logic for the received only Excel file
        if (UploadedFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select a valid Excel file.";
            return RedirectToPage();
        }
        var extension = Path.GetExtension(UploadedFile.FileName).ToLower();
        var allowedExtensions = new[] { ".xlsx", ".xls", ".csv" };
        if (!allowedExtensions.Contains(extension))
        {
            TempData["ErrorMessage"] = "Please select a valid Excel file.";
            return  RedirectToPage();
        }
        if (UploadedFile.Length > 0)
        {
            using var stream = UploadedFile.OpenReadStream();

            // Obtenin the products count and import them, if only don't be in the database and don't have duplicate
            int count = await excelService.ImportProductsAsync(stream);
            
            if (count > 0)
            {
                TempData["SuccessMessage"] = $"Success! {count} new products imported successfully.";
            }
            else
            {
               
                TempData["InfoMessage"] = "No new products were imported. They might be duplicates.";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Please select a valid Excel file.";
        }

        return RedirectToPage();
    }
}