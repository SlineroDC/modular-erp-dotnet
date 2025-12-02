using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Pages.Products;

public class Create(IProductRepository productRepository) : PageModel
{
   //Connect to inputs with forms
   [BindProperty] public Product Product { get; set; } = new();
   
   public void OnGet()
   { 
      if( Product.Price >= 0);
      {
         
      }
   }

   public async Task<IActionResult> OnPostAsync()
   {
      // Capture to errors in forms
      if (!ModelState.IsValid)
      {
         return Page(); 
      }

      if (Product.Price < 0)
      {
         ModelState.AddModelError("Product.Price", "The product price must be greater than or equal to 0.");
         
         ViewData["ErrorMessage"] = "The product price cannot be negative.";
         return Page();
      }

      try
      {
         await productRepository.AddAsync(Product);
      }
      catch (Exception ex)
      {
         ModelState.AddModelError(string.Empty, "An error occurred while saving.");
         
         ViewData["ErrorMessage"] = ex.Message;
         
         return Page();
      throw;
      }
     
      return RedirectToPage("./Index");
   }
}