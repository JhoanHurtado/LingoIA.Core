using LingoIA.Domain.Enums;

namespace LingoIA.Application.Dtos
{
    public class MessageDto
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public Guid User { get; set; }
        public Guid? ConversationId { get; set; } = Guid.NewGuid();
        public EnumSender Sender { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? CorrectedText { get; set; }
        public decimal Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Token { get; set; } = string.Empty;
        public string? MispronouncedWord { get; set; }
        public string? AssistantResponse { get; set; }
        public string Explanation { get; set; } = string.Empty;
    }
}