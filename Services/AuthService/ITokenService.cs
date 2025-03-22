using LibraryProject.Models;
using OrderApiProject_week2.Models;

namespace LibraryProject.Services.AuthService
{
    public interface ITokenService
    {
        string GenerateJwtToken(string username, string role);
        Task<RefreshToken> GenerateRefreshToken(int userID, string role);

        Task<User> GetUserByRefreshToken(string refreshToken);
        Task UpdateRefreshToken(RefreshToken refreshToken);
    }
}
