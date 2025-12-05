using System.Security.Claims;
using ERP.Api.Dto;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISalesRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    public SalesController(
        ISalesRepository saleRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IPdfService pdfService,
        IEmailService emailService
    )
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _pdfService = pdfService;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSale([FromBody] SaleDetailDto saleDto)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var customers = await _customerRepository.GetAllAsync(1, 1, userEmail);
        var customer = customers.Items.FirstOrDefault();

        if (customer == null)
            return BadRequest("Client not found in the business database.");

        // 2. Construir la Venta
        var sale = new Sale
        {
            CustomerId = customer.Id,
            Date = DateTime.UtcNow,
            IsActive = true,
            SalesDetails = new List<SalesDetail>(),
        };

        decimal total = 0;

        // 3. Procesar Productos
        foreach (var item in saleDto.salesDetails)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                return BadRequest($"Product {item.ProductId} don't found.");
            if (product.Stock < item.Quantity)
                return BadRequest($"Insufficient stock for {product.Name}.");

            var detail = new SalesDetail
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
            };

            sale.SalesDetails.Add(detail);
            total += detail.Quantity * detail.UnitPrice;

            product.Stock -= item.Quantity;
            await _productRepository.UpdateAsync(product);
        }

        sale.Total = total;

        await _saleRepository.AddAsync(sale);

        try
        {
            var fullSale = await _saleRepository.GetByIdAsync(sale.Id);
            var pdfBytes = _pdfService.GenerateSaleReceipt(fullSale!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }

        return Ok(new { message = "Successful purchase", saleId = sale.Id });
    }
}
