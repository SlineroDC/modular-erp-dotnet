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
            ModelState.AddModelError(string.Empty, "Error: The Document or Email is already registered.\"");
            return Page();
        }
        
        try
        {
            await customerRepository.AddAsync(Customer);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving. Check the data.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}