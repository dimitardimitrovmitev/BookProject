using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookProject.Models;
using Microsoft.IdentityModel.Tokens;

namespace BookProject.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_config["Jwt:ExpiryHours"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
