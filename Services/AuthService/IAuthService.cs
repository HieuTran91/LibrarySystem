using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.AuthService
{
    public interface IAuthService
    {
        Task<UserDTO> AuthenticateAsync(LoginDTO loginDTO);
        Task<UserDTO> AddNewUser(RegisterDTO registerDTO);
    }
}
