using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repositories.UserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<Reader> GetByNameAsync(string name);
        //Task<Librarian> GetLibrarianByNameAsync(string name);
        //Task<IEnumerable<Reader>> GetAllReadersAsync();
        //Task<IEnumerable<Librarian>> GetAllLibrariansAsync();
        //Task<UserDTO> UpdateUserAsync(RegisterDTO registerDTO);
        //Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
