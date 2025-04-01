using System.ComponentModel.DataAnnotations;
using LingoIA.Domain.Enums;

namespace LingoIA.Application.Dtos
{
    public class RegisterUserDto
    {
       [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string PreferredLanguage { get; set; } = "en";
        
        public LanguageLevel Level { get; set; } = LanguageLevel.Beginner;
    }
}