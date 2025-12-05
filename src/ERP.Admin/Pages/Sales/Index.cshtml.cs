using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP.Admin.Pages;

public class SalesModel(ISalesRepository salesRepository, IPdfService pdfService) : PageModel
{
    private readonly ISalesRepository _salesRepository = salesRepository;
    private readonly IPdfService _pdfService = pdfService;

    public ResponsePage<Sale> Data { get; set; } = new();

    public async Task OnGetAsync([FromQuery] int pageNumber = 1)
    {
        Data = await _salesRepository.GetPaginatedAsync(pageNumber, 10);
    }

    //method download PDF

    public async Task<IActionResult> OnGetDownloadPdfAsync(int id)
    {
        var sale = await _salesRepository.GetByIdAsync(id);

        if (sale == null)
            return NotFound();

        var pdfBytes = _pdfService.GenerateSaleReceipt(sale);

        return File(pdfBytes, "application/pdf", $"Receipt_Sale_ {id} .pdf");
    }
}
