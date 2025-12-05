using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ERP.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ERP.Infrastructure.Services;

public class GeminiAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    private readonly Dictionary<string, string> _navigationMap = new()
    {
        { "/Index", "Dashboard: View general metrics." },
        { "/Products", "Products list: View inventory." },
        { "/CreateProduct", "Create product." },
        { "/Customers", "Customers." },
        { "/CreateCustomer", "Create customer." },
        { "/Sales", "Sales history." },
        { "/CreateSale", "New sale (POS)." },
        { "/Settings", "Settings." },
    };

    public GeminiAiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["AiSettings:BaseUrl"]!);
        _apiKey = configuration["AiSettings:ApiKey"]!;
        _model = configuration["AiSettings:Model"] ?? "gemini-1.5-flash";
    }

    public async Task<string> AskAssistantAsync(string userQuestion)
    {
        // 1. Prepare system prompt (context)
        var navigationContext = string.Join(
            "\n",
            _navigationMap.Select(x => $"- Route '{x.Key}': {x.Value}")
        );
        var systemInstruction =
            $@"
            You are 'FirmezaBot', the ERP assistant.
            SYSTEM KNOWLEDGE:
            {navigationContext}
            RULES:
            - Respond in English, briefly.
            - If asked to navigate to a location, reply with this exact HTML format: <a href='/Route' class='text-yellow-300 underline font-bold'>Name</a>.
        ";

        // 2. Create request body (Google Gemini specific format)
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = $"{systemInstruction}\n\nUser: {userQuestion}\nAssistant:" },
                    },
                },
            },
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var fullUrl = $"{_httpClient.BaseAddress}{_model}:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(fullUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {error}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GeminiResponse>(jsonString);

            // Extract the text from Gemini's response
            return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text
                ?? "No response.";
        }
        catch (Exception ex)
        {
            return $"AI error: {ex.Message}";
        }
    }

    private class GeminiResponse
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }
    }

    private class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }

    private class Content
    {
        [JsonPropertyName("parts")]
        public List<Part>? Parts { get; set; }
    }

    private class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
