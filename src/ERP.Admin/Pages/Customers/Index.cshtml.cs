using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Pages.Customers;

public class CustomerModel(ICustomerRepository customerRepository) : PageModel
{
    public ResponsePage<Customer> Data { get; set; } = new();
    
    // Feature search customers bar
    [BindProperty(SupportsGet = true)] public string? SearchTerm { get; set; }
    
    public async Task OnGetAsync([FromQuery] int pageNumber = 1)
    {
        const int pageSize = 10;
        Data = await customerRepository.GetAllAsync(pageNumber, pageSize, SearchTerm);
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await customerRepository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Customer deleted successfully";
        return RedirectToPage();
    }
}