using LibraryProject.Models;
using LibraryProject.Repositories.LoginRepository;
using Microsoft.IdentityModel.Tokens;
using OrderApiProject_week2.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryProject.Services.AuthService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        public TokenService(IConfiguration configuration, IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }
        public async Task<RefreshToken> GenerateRefreshToken(int userID, string role)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserID = userID,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _authRepository.AddRefreshToken(refreshToken);
            return refreshToken;
        }

        public string GenerateJwtToken(string username, string role)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username), "Username cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException(nameof(role), "Role cannot be null or empty.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };
            Console.WriteLine(claims);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:TokenExpirationMinutes"])),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            return await _authRepository.GetUserByRefreshToken(refreshToken);
        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            await _authRepository.UpdateRefreshTokenAsync(refreshToken);
        }
    }
}
