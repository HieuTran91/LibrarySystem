using LibraryProject.DTOs;
using LibraryProject.Models;
using Microsoft.AspNetCore.Identity.Data;
using OrderApiProject_week2.Models;

namespace LibraryProject.Repositories.LoginRepository
{
    public interface IAuthRepository
    {
        //Task GetUsername(string username);
        Task<User> GetLoginAsync(string username);
        Task AddNewUserAsync(User user);
        Task<RefreshToken> GetRefreshToken(string refreshToken);
        Task AddRefreshToken(RefreshToken refreshToken);
        Task<User> GetUserByRefreshToken(string refreshToken);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
