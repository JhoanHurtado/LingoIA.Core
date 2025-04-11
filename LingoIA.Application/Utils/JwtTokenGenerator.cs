using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LingoIA.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LingoIA.Application.Utils
{
    public static class JwtTokenGenerator
    {
        private const string SecretKey = "clave-super-secreta78567yuhjkg87giuiui"; // Cambia esto por una clave segura
        private const int ExpirationMinutes = 60;

        public static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "LingoIA",
                audience: "LingoIAUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = "LingoIA",
                ValidateAudience = true,
                ValidAudience = "LingoIAUsers",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("Token expirado.");
            }
            catch (SecurityTokenException)
            {
                Console.WriteLine("Token inválido.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al validar el token: {ex.Message}");
            }

            return null;
        }
    }
}