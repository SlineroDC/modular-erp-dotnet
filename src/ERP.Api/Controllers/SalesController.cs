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
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto saleDto)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(userEmail))
            return Unauthorized();

        var customers = await _customerRepository.GetAllAsync(1, 1, userEmail);
        var customer = customers.Items.FirstOrDefault();

        if (customer == null)
            return BadRequest("Client not found in the business database.");

        var sale = new Sale
        {
            CustomerId = customer.Id,
            Date = DateTime.UtcNow,
            IsActive = true,
            SalesDetails = new List<SalesDetail>(),
        };

        decimal total = 0;

        foreach (var item in saleDto.Details)
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

            if (fullSale != null && fullSale.Customer != null)
            {
                var pdfBytes = _pdfService.GenerateSaleReceipt(fullSale);

                var emailBody =
                    $@"
                     <h3>Thank you for your purchase, {fullSale.Customer.Name}!</h3>
                    <p>Attached you will find the receipt of your purchase made on {fullSale.Date}.</p>
                    <p>Total: <strong>{fullSale.Total:C}</strong></p>
                    <br>
                    <p>Sincerely,<br>The Firmeza team.</p>
                ";

                await _emailService.SendEmailWithAttachmentAsync(
                    toEmail: fullSale.Customer.Email,
                    subject: $"Recibo de Compra #{fullSale.Id} - Firmeza",
                    message: emailBody,
                    attachmentFile: pdfBytes,
                    fileName: $"Recibo_{fullSale.Id}.pdf"
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }

        return Ok(new { message = "Successful purchase", saleId = sale.Id });
    }
}
