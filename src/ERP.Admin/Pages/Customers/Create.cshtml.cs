using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Pages.Customers;

public class CreateCustomerModel(ICustomerRepository customerRepository) : PageModel
{
    [BindProperty]
    public Customer Customer { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        bool isDuplicate = await customerRepository.IsDuplicateAsync(Customer.Email, Customer.IdDocument);

        if (isDuplicate)
        {
            ModelState.AddModelError(string.Empty, "Error: El Documento o Email ya están registrados.");
            return Page();
        }
        
        try
        {
            await customerRepository.AddAsync(Customer);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar. Verifique los datos.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}