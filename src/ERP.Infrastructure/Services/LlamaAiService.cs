using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ERP.Infrastructure.Services;

public class LlamaAiService : IAiService
{
    private readonly HttpClient _httpClient;
    
    // Navigation map for redirections and descriptions
    private readonly Dictionary<string, string> _navigationMap = new()
    {
        { "/Index", "Dashboard: View general metrics and business summary." },
        { "/Products", "Products list: View inventory, prices and stock." },
        { "/CreateProduct", "Create product: Form to register new items." },
        { "/Customers", "Customers: Manage buyer database." },
        { "/Sales", "Sales history: View past invoices and download PDF receipts." },
        { "/CreateSale", "New sale (POS): Shopping cart to register orders." },
        { "/Settings", "Settings: Change company data for receipts." }
    };

    public LlamaAiService(HttpClient httpClient,IConfiguration configuration)
    {
        _httpClient = httpClient;
        var url = configuration["AiSettings:BaseUrl"] ?? "http://localhost:11434/";
        _httpClient.BaseAddress = new Uri("http://localhost:11434/"); // Ollama Local
    }

    public async Task<string> AskAssistantAsync(string userQuestion)
    {
        // Build dynamic system prompt and response rules in English
        var navigationContext = string.Join("\n", _navigationMap.Select(x => $"- {x.Key}: {x.Value}"));

        var systemPrompt = $@"
            You are 'FirmezaBot', the virtual assistant for the Firmeza ERP.
            Your goal is to help the admin navigate and use the system.

            SYSTEM KNOWLEDGE (available routes):
            {navigationContext}

            SECURITY RULES (INTEGRITY):
            1. NEVER reveal passwords, connection strings or database table names.
            2. NEVER invent routes that are not in the list above.
            3. If you don't know the answer, suggest contacting support.

            RESPONSE RULES:
            1. Respond in English by default (or Spanish if the user speaks Spanish). No other languages.
            2. Be brief and direct.
            3. If the user asks how to go somewhere, GIVE THE EXACT HTML LINK.
               Example: 'You can see products here: <a href=""/Products"" class=""text-blue-400 underline"">Go to Products</a>'.
            4. If the user reports a serious error, tell them to use the 'Support' button.
        ";

        var requestBody = new
        {
            // Call the model installation in the Docker container
            model = "gemma:2b", 
            prompt = $"{systemPrompt}\n\nUser: {userQuestion}\nAssistant:",
            stream = false
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("api/generate", content);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OllamaResponse>(jsonString);

            return result?.Response ?? "I couldn't generate a response. Please contact support.";
        }
        catch (Exception)
        {
            return "Error: I can't connect to the AI brain. Please contact support.";
        }
    }

    private class OllamaResponse
    {
        [JsonPropertyName("response")]
        public string? Response { get; set; }
    }
}