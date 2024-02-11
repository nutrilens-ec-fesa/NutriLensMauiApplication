using Microsoft.IdentityModel.Tokens;
using NutriLensClassLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NutriLensWebApp.Services
{
    public static class TokenService
    {
        private static string _secret;
        private static int _hoursToExpire;

        public static void UpdateSecret(string secret)
        {
            _secret = secret;
        }

        public static void UpdateHoursToExpire(int hoursToExpire)
        {
            _hoursToExpire = hoursToExpire;
        }

        public static string GenerateToken(Login user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid, user.UserInfoIdentifier)
                }),
                Expires = DateTime.UtcNow.AddHours(_hoursToExpire),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
