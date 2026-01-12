namespace GeminiChat.API.Services;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"] ?? "";
    }

    public async Task<string> GetGeminiResponse(string userPrompt)
    {
        // 1. DEFINING THE MODEL (We use gemini-1.5-flash here)
        var modelId = "gemini-2.5-flash";
        var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{modelId}:generateContent?key={_apiKey}";

        // 2. DEBUG PRINTING (Check your terminal for this!)
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine($"[DEBUG] Using API Key: '{_apiKey}'");
        Console.WriteLine($"[DEBUG] Request URL: {requestUrl}");
        Console.WriteLine("--------------------------------------------------");

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = userPrompt } } }
            }
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(requestUrl, jsonContent);
        
        if (!response.IsSuccessStatusCode)
        {
            // Print the full error from Google to the console
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ERROR] Google API Error: {errorContent}");
            return "Error calling Gemini API: " + response.ReasonPhrase;
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<GeminiResponseRoot>(jsonResponse);

        return data?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "No response generated.";
    }
}

// Helper classes
public class GeminiResponseRoot
{
    [JsonPropertyName("candidates")]
    public List<Candidate>? Candidates { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content? Content { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part>? Parts { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}