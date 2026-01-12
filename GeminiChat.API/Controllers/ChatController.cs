using GeminiChat.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeminiChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly GeminiService _geminiService;

    public ChatController(GeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty");

        var answer = await _geminiService.GetGeminiResponse(request.Message);
        return Ok(new { Reply = answer });
    }
}

public class ChatRequest
{
    public string? Message { get; set; }
}