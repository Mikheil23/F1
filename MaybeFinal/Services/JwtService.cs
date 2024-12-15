using MaybeFinal.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaybeFinal.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user)
        {
            // Ensure the User object has a valid role (using "User" role as fallback if not set)
            var role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;  // Default to "User" if Role is not set

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),  // Correct access to UserName here
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                new Claim(ClaimTypes.Role, role)  // Role
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expirationTime = DateTime.Now.AddDays(_jwtSettings.ExpirationDays);  // Assuming ExpirationDays is in settings

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expirationTime,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}




