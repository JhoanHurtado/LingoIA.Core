using System.Text.Json.Serialization;

namespace LingoIA.Domain.models
{
    public class MessageAnalysis
    {
        [JsonPropertyName("original")]
        public string Original { get; set; } = string.Empty;

        [JsonPropertyName("corrected")]
        public string Corrected { get; set; } = string.Empty;

        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public decimal Score { get; set; } = 10;
    }
}
