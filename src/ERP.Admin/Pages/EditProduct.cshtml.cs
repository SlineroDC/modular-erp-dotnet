using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Admin.Pages;

public class EditProduct(IProductRepository productRepository) : PageModel
{
   [BindProperty] public Product Product { get; set; } = new();

   //Load data
   public async Task<IActionResult> OnGetAsync(int id)
   {
      var product = await productRepository.GetByIdAsync(id);
      if (product == null)
      {
         return NotFound(); //If ID dont exist, 404
      }

      Product = product;
      return Page();
   }

   // Saved changes data
   public async Task<IActionResult> OnPostAsync()
   {
      if (!ModelState.IsValid)
      {
         return  Page();
      }
      await  productRepository.UpdateAsync(Product);
      
      return  RedirectToPage("./Products");
   }

}