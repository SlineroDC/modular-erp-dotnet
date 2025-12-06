using System.Net;
using System.Net.Mail;
using ERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ERP.Infrastructure.Services;

public class SmtpEmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string subject, string messageBody)
    {
        // 1. Read the settings

        var smtpServer = configuration["EmailSettings:Server"];
        var portString = configuration["EmailSettings:Port"];
        var senderEmail = configuration["EmailSettings:SenderEmail"];
        var password = configuration["EmailSettings:Password"];

        var adminEmail = configuration["EmailSettings:AdminEmail"];

        // 2. Security validations to avoid crashing
        if (
            string.IsNullOrEmpty(smtpServer)
            || string.IsNullOrEmpty(senderEmail)
            || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(adminEmail)
        )
        {
            throw new InvalidOperationException("Missing email configurations in appsettings.json");
        }

        if (!int.TryParse(portString, out int port))
        {
            throw new InvalidOperationException(
                "The mail port in appsettings.json is not a valid number."
            );
        }

        // 3. Configure the SMTP Client
        using var client = new SmtpClient(smtpServer, port);
        client.Credentials = new NetworkCredential(senderEmail, password);
        client.EnableSsl = true;

        // 4. Create the Message
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, "ERP Support"),
            Subject = $"[Support ERP] {subject}",
            Body = messageBody,
            IsBodyHtml = true,
        };

        // 5. Add the recipient
        mailMessage.To.Add(adminEmail);

        // 6. Send
        await client.SendMailAsync(mailMessage);
    }

    public async Task SendEmailWithAttachmentAsync(
        string toEmail,
        string subject,
        string message,
        byte[] attachmentFile,
        string fileName
    )
    {
        await SendEmailInternalAsync(toEmail, subject, message, attachmentFile, fileName);
    }

    // Private helper method to avoid repeating code
    private async Task SendEmailInternalAsync(
        string toEmail,
        string subject,
        string body,
        byte[]? attachmentData,
        string? attachmentName
    )
    {
        var smtpServer = configuration["EmailSettings:Server"];
        var port = int.Parse(configuration["EmailSettings:Port"]!);
        var senderEmail = configuration["EmailSettings:SenderEmail"];
        var password = configuration["EmailSettings:Password"];

        if (
            string.IsNullOrEmpty(smtpServer)
            || string.IsNullOrEmpty(senderEmail)
            || string.IsNullOrEmpty(password)
        )
        {
            throw new InvalidOperationException("Missing email configurations in appsettings.json");
        }

        using var client = new SmtpClient(smtpServer, port);
        client.Credentials = new NetworkCredential(senderEmail, password);
        client.EnableSsl = true;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, "Firmeza ERP"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        if (attachmentData != null && !string.IsNullOrEmpty(attachmentName))
        {
            var stream = new MemoryStream(attachmentData);
            var attachment = new Attachment(stream, attachmentName, "application/pdf");
            mailMessage.Attachments.Add(attachment);
        }

        await client.SendMailAsync(mailMessage);
    }
}
