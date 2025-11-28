namespace ERP.Core.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string message);
}