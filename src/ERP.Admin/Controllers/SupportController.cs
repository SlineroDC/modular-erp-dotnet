using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SupportController(IEmailService emailService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendSupportRequest([FromBody] SupportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message)) return BadRequest();

        var body = $@"
            <h3>Nuevo mensaje de soporte</h3>
            <p><strong>Usuario:</strong> {User.Identity?.Name ?? "Anónimo"}</p>
            <p><strong>Mensaje:</strong></p>
            <p>{request.Message}</p>
        ";

        try
        {
            await emailService.SendEmailAsync("Ayuda desde el Dashboard", body);
            return Ok(new { message = "Correo enviado exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.ToString() });
        }
    }

    public class SupportRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}