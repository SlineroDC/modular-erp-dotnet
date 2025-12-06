using ERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Admin.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AiController(IAiService aiService) : ControllerBase
{
    private readonly IAiService _aiService = aiService;

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest();

        var answer = await _aiService.AskAssistantAsync(request.Question);
        return Ok(new { answer });
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
