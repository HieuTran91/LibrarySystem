using LibraryProject.Models;

namespace LibraryProject.Services.UserService
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
    }
}
