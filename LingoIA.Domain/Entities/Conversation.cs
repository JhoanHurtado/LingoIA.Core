using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LingoIA.Domain.Entities
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(225)]
        public required string Topic { get; set; }

        [MaxLength(10)]
        public string Language { get; set; } = "en";

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // Relación con Messages
        public List<Message> Messages { get; set; } = new();
    }
}