using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Pages.Customers;

public class EditCustomerModel(ICustomerRepository customerRepository) : PageModel
{
    [BindProperty]
    public Customer Customer { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        Customer = customer;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var isDuplicate = await customerRepository.IsDuplicateAsync(Customer.Email, 
            Customer.IdDocument,
            Customer.Id);

        if (isDuplicate)
        {
           
            ModelState.AddModelError(string.Empty, "Error: The Document or Email already belongs to another client.");
            return Page(); 
        }
        
        try
        {
            await customerRepository.UpdateAsync(Customer);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while saving to the database. Verify that the data does not exist.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}