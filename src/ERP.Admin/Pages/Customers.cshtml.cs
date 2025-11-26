using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Admin.Pages;

public class CustomerModel(ICustomerRepository customerRepository) : PageModel
{
    public ResponsePage<Customer> Data { get; set; } = new();
    
    public async Task OnGetAsync([FromQuery] int pageNumber = 1)
    {
        const int pageSize = 10;
        Data = await customerRepository.GetAllAsync(pageNumber, pageSize);
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await customerRepository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Customer deleted successfully";
        return RedirectToPage();
    }
}