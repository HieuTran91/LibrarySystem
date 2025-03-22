using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.UserService
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> GetUserByNameAsync(string name);
        Task DeleteUserAsync(int id);
        Task<UserDTO> UpdateUserAsync(RegisterDTO registerDTO);
        Task<UserDTO> AddUserAsync(RegisterDTO registerDTO);
        Task<IEnumerable<UserDTO>> GetAllUserAsync();
    }
}
