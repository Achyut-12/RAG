using System.Text.RegularExpressions;

public static class TextChunker
{
    // Very simple token counter (word-based)
    static int CountTokens(string text)
    {
        return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static IEnumerable<string> ChunkText(
        string text,
        int maxTokens = 300,
        int overlapTokens = 50)
    {
        // 1. Split by sentences
        var sentences = Regex.Split(text, @"(?<=[\.!\?])\s+");

        var chunks = new List<string>();
        var current = new List<string>();
        int currentTokens = 0;

        foreach (var sentence in sentences)
        {
            int sentenceTokens = CountTokens(sentence);

            // If adding this sentence exceeds the chunk limit → flush current chunk
            if (currentTokens + sentenceTokens > maxTokens)
            {
                // add completed chunk
                chunks.Add(string.Join(" ", current));

                // 2. Apply overlap: keep last X tokens from previous chunk
                if (overlapTokens > 0)
                {
                    var overlap = string.Join(" ", current)
                        .Split(" ")
                        .Reverse()
                        .Take(overlapTokens)
                        .Reverse()
                        .ToList();

                    current = new List<string> { string.Join(" ", overlap) };
                    currentTokens = CountTokens(current[0]);
                }
                else
                {
                    current = new List<string>();
                    currentTokens = 0;
                }
            }

            current.Add(sentence);
            currentTokens += sentenceTokens;
        }

        // Add final chunk
        if (current.Count > 0)
            chunks.Add(string.Join(" ", current));

        return chunks;
    }
}
