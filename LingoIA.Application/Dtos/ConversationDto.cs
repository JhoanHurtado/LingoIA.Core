using System;

namespace LingoIA.Application.Dtos
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Topic { get; set; }
        public string? Language { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
