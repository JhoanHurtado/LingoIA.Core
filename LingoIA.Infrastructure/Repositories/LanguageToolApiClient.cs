using System.Net.Http.Json;
using System.Text.Json;
using LingoIA.Domain.models;
using LingoIA.Infrastructure.Interfaces;

namespace LingoIA.Infrastructure.Repositories
{
    public class LanguageToolApiClient : ILanguageToolApiClient
    {
        private readonly HttpClient _httpClient;

        public LanguageToolApiClient()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://api.languagetool.org/v2/") };
        }

        public async Task<CorrectionResult> CheckTextAsync(string text, string language)
        {
            var response = await _httpClient.PostAsync("check", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "text", text },
                { "language", language }
            }));

            if (!response.IsSuccessStatusCode)
                return new CorrectionResult { OriginalText = text, CorrectedText = text };

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var document = JsonDocument.Parse(jsonResponse);
            var root = document.RootElement;

            if (!root.TryGetProperty("matches", out var matches))
                return new CorrectionResult { OriginalText = text, CorrectedText = text };

            var errors = new List<string>();
            string correctedText = text;

            foreach (var match in matches.EnumerateArray())
            {
                if (match.TryGetProperty("message", out var message) &&
                    match.TryGetProperty("replacements", out var replacements) &&
                    match.TryGetProperty("context", out var context) &&
                    match.TryGetProperty("offset", out var offset) &&
                    match.TryGetProperty("length", out var length) &&
                    replacements.GetArrayLength() > 0)
                {
                    string error = $"Error: {message.GetString()}, Sugerencia: {replacements[0].GetProperty("value").GetString()}";
                    errors.Add(error);

                    string originalWord = text.Substring(offset.GetInt32(), length.GetInt32());
                    string replacementWord = replacements[0].GetProperty("value").GetString() ?? originalWord;

                    correctedText = correctedText.Replace(originalWord, replacementWord);
                }
            }

            return new CorrectionResult
            {
                OriginalText = text,
                CorrectedText = correctedText,
                Errors = errors
            };
        }
    }
}