namespace ERP.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string message);
    Task SendEmailWithAttachmentAsync(
        string toEmail,
        string subject,
        string message,
        byte[] attachmentFile,
        string fileName
    );
}
