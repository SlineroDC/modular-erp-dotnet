using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ERP.Core.Interfaces;

namespace ERP.Infrastructure.Services;

public class LlamaAiService : IAiService
{
    private readonly HttpClient _httpClient;
    
    //Navigation Map for redirections and descriptions
    private readonly Dictionary<string, string> _navigationMap = new()
    {
        { "/Index", "Dashboard: Ver métricas generales y resumen del negocio." },
        { "/Products", "Lista de Productos: Ver inventario, precios y stock." },
        { "/CreateProduct", "Crear Producto: Formulario para registrar nuevos items." },
        { "/Customers", "Clientes: Gestionar la base de datos de compradores." },
        { "/Sales", "Historial de Ventas: Ver facturas pasadas y descargar recibos PDF." },
        { "/CreateSale", "Nueva Venta (POS): Carrito de compras para registrar pedidos." },
        { "/Settings", "Configuración: Cambiar datos de la empresa para los recibos." }
    };

    public LlamaAiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:11434/"); // Ollama Local
    }

    public async Task<string> AskAssistantAsync(string userQuestion)
    {
        // Constructor prompt dynamic the system in spanish and response too in English
        var navigationContext = string.Join("\n", _navigationMap.Select(x => $"- {x.Key}: {x.Value}"));

        var systemPrompt = $@"
            Eres 'FirmezaBot', el asistente virtual del ERP Firmeza.
            Tu objetivo es ayudar al administrador a navegar y usar el sistema.

            CONOCIMIENTO DEL SISTEMA (Rutas disponibles):
            {navigationContext}

            REGLAS DE SEGURIDAD (INTEGRIDAD):
            1. NUNCA reveles contraseñas, cadenas de conexión o nombres de tablas de la base de datos.
            2. NUNCA inventes rutas que no estén en la lista de arriba.
            3. Si no sabes la respuesta, sugiere contactar a soporte.

            REGLAS DE RESPUESTA:
            1. Responde solo en español o en ingles (NO MAS IDOMAS) por defecto ingles a no ser que te hablen en español.
            2. Sé breve y directo.
            3. Si el usuario pregunta cómo ir a un lugar, DALE EL LINK HTML exacto.
               Ejemplo: 'Puedes ver los productos aquí: <a href=""/Products"" class=""text-blue-400 underline"">Ir a Productos</a>'.
            4. Si el usuario reporta un error grave, dile que use el botón de 'Soporte'.
        ";

        var requestBody = new
        {
            // Call the model installation in the Docker container
            model = "gemma:2b", 
            prompt = $"{systemPrompt}\n\nUsuario: {userQuestion}\nAsistente:",
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
        public string Response { get; set; }
    }
}