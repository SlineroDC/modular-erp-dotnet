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
        { "/Index", "Dashboard: Ver métricas generales." },
        { "/Products", "Lista de Productos: Ver inventario." },
        { "/CreateProduct", "Crear Producto." },
        { "/Customers", "Clientes." },
        { "/CreateCustomer", "Crear Cliente." },
        { "/Sales", "Historial de Ventas." },
        { "/CreateSale", "Nueva Venta (POS)." },
        { "/Settings", "Configuración." }
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
        // 1. Preparar el Prompt de Sistema (Contexto)
        var navigationContext = string.Join("\n", _navigationMap.Select(x => $"- Ruta '{x.Key}': {x.Value}"));
        var systemInstruction = $@"
            Eres 'FirmezaBot', el asistente del ERP.
            CONOCIMIENTO DEL SISTEMA:
            {navigationContext}
            REGLAS:
            - Responde en Español, brevemente.
            - Si te piden ir a un lugar, responde con este formato HTML exacto: <a href='/Ruta' class='text-yellow-300 underline font-bold'>Nombre</a>.
        ";

        // 2. Crear el cuerpo de la petición (Formato específico de Google Gemini)
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = $"{systemInstruction}\n\nUsuario: {userQuestion}\nAsistente:" }
                    }
                }
            }
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

            // Extraer el texto de la respuesta de Gemini
            return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No respuesta.";
        }
        catch (Exception ex)
        {
            return $"Error de IA: {ex.Message}";
        }
    }

    // Clases para deserializar la respuesta de Google
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