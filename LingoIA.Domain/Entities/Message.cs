using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LingoIA.Domain.Enums;

namespace LingoIA.Domain.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("Conversation")]
        public Guid ConversationId { get; set; }

        [Required]
        public required EnumSender Sender { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public required string Text { get; set; }

        [Column(TypeName = "text")]
        public string? CorrectedText { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "numeric(5,2)")]
        public decimal Score { get; set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Relación con Conversation
        public Conversation? Conversation { get; set; }
    }


}