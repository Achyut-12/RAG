using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

public class GenerateAnswer
{

    public static async Task<string> GenerateAnswerAsync(string promptText)
    {
        
        var client = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:11500/")
        };

        var payload = new
        {
            model = "phi",  // check your actual model name
            prompt = promptText
        };

        var response = await client.PostAsJsonAsync("api/generate", payload);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error {response.StatusCode}: {error}");
        }

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