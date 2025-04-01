

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LingoIA.Domain.Enums;

namespace LingoIA.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string PasswordHash { get; private set; } = string.Empty;

        public string PreferredLanguage { get; set; } = "en";

        [MaxLength(20)]
        public LanguageLevel Level { get; set; } = LanguageLevel.Beginner;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Establece y hashea la contraseña automáticamente al asignarla
        /// </summary>
        public string Password
        {
            set => PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
        }

        /// <summary>
        /// Método público para cambiar la contraseña de forma segura
        /// </summary>
        public void ChangePassword(string newPassword)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }

        /// <summary>
        /// Verifica si una contraseña dada coincide con la almacenada
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}