using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Admin.Pages;

public class CreateProduct(IProductRepository productRepository) : PageModel
{
   //Connect to inputs with forms
   [BindProperty] public Product Product { get; set; } = new();

   public void OnGet()
   {
      var ProctectedNegative = Product.Price >= 0;
   }

   public async Task<IActionResult> OnPostAsync()
   {
      // Capture to errors in forms
      if (!ModelState.IsValid)
      {
         return Page(); 
      }
      await productRepository.AddAsync(Product);
      return RedirectToPage("./Products");
   }
}