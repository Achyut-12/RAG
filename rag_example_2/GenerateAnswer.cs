using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public class GenerateAnswer
{

    public static async Task<string> GenerateAnswerAsync(string promptText)
    {
        using var client = new HttpClient { BaseAddress = new Uri("http://localhost:11434/") };

        var payload = new { model = "phi", prompt = promptText };
        var response = await client.PostAsJsonAsync("api/generate", payload);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        // Ollama streams JSON lines, merge them
        var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            try
            {
                var json = JsonDocument.Parse(line);
                if (json.RootElement.TryGetProperty("response", out var r))
                    sb.Append(r.GetString());
            }
            catch
            {
                // ignore invalid JSON fragments
            }
        }

        return sb.ToString().Trim();
    }
}