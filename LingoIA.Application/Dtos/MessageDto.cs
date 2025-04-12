using LingoIA.Domain.Enums;

namespace LingoIA.Application.Dtos
{
    public class MessageDto
    {
        public Guid? Id { get; set; }
        public Guid User { get; set; }
        public Guid? ConversationId { get; set; }
        public EnumSender Sender { get; set; }
        public string Text { get; set; } = string.Empty;
        public string CorrectedText { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Token { get; set; } = string.Empty;
        public string MispronouncedWord { get; set; } = string.Empty;
        public string AssistantResponse { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public string Language { get; set; } = "en";
    }
}