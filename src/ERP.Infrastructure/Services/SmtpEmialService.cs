using System.Net;
using System.Net.Mail;
using ERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ERP.Infrastructure.Services;

public class SmtpEmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string subject, string messageBody)
    {
        // 1. Leer la configuración
        // Usamos el operador ?? "" para evitar nulos, aunque lo ideal es validar que existan.
        var smtpServer = configuration["EmailSettings:Server"];
        var portString = configuration["EmailSettings:Port"];
        var senderEmail = configuration["EmailSettings:SenderEmail"];
        var password = configuration["EmailSettings:Password"];
        
        // CORRECCIÓN AQUÍ: Usamos la clave "AdminEmail"
        var adminEmail = configuration["EmailSettings:AdminEmail"]; 

        // 2. Validaciones de seguridad para no crashear
        if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(adminEmail))
        {
            throw new InvalidOperationException("Faltan configuraciones de correo en appsettings.json");
        }

        if (!int.TryParse(portString, out int port))
        {
             throw new InvalidOperationException("El puerto de correo en appsettings.json no es un número válido.");
        }

        // 3. Configurar el Cliente SMTP
        using var client = new SmtpClient(smtpServer, port);
        client.Credentials = new NetworkCredential(senderEmail, password);
        client.EnableSsl = true;

        // 4. Crear el Mensaje
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, "ERP Support"),
            Subject = $"[Support ERP] {subject}",
            Body = messageBody,
            IsBodyHtml = true
        };

        // 5. Agregar el destinatario (Esto es lo que fallaba antes)
        mailMessage.To.Add(adminEmail);

        // 6. Enviar
        await client.SendMailAsync(mailMessage);
    }
}