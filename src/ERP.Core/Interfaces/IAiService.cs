namespace ERP.Core.Interfaces;

public interface IAiService
{
    Task<string> AskAssistantAsync(string UserQuestion);
}