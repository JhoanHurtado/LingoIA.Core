using LingoIA.Domain.Enums;

namespace LingoIA.Application.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public Guid ConversationId { get; set; }
        public EnumSender Sender { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? CorrectedText { get; set; }
        public decimal Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}