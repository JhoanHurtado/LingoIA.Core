using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LingoIA.Domain.Entities
{
    public class LearningData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!; // Relación con la tabla User

        [Required]
        public string Phrase { get; set; } = string.Empty;

        [Required]
        public string Correction { get; set; } = string.Empty;

        public int UsageCount { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}