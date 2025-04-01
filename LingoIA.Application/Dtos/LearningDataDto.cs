namespace LingoIA.Application.Dtos
{
    public class LearningDataDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Phrase { get; set; } = string.Empty;
        public string Correction { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}