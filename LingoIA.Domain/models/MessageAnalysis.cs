using System.Text.Json.Serialization;

namespace LingoIA.Domain.models
{
    public class MessageAnalysis
    {
        [JsonPropertyName("original")]
        public string? Original { get; set; }

        [JsonPropertyName("corrected")]
        public string? Corrected { get; set; }

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public int Score { get; set; } = 10;
    }
}
