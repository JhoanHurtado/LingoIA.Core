namespace LingoIA.Domain.models
{
    public class CorrectionResult
    {
        public string OriginalText { get; set; } = string.Empty;
        public string CorrectedText { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }
}