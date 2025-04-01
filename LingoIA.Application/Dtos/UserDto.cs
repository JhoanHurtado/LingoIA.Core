using LingoIA.Domain.Enums;

namespace LingoIA.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PreferredLanguage { get; set; } = "en";
        public string? Token { get; set; }
        public LanguageLevel Level { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}