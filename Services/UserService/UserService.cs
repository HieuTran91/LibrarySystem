using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
//using LibraryProject.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _user;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<object> _passwordHasher;
        private readonly IRepository<Role> _roleRepository;

        public UserService(IRepository<User> user, IMapper mapper, IPasswordHasher<object> passwordHasher, IRepository<Role> roleRepository)
        {
            _user = user;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
        }

        public async Task<UserDTO> AddUserAsync(RegisterDTO registerDTO)
        {
            var users = await _user.GetAllAsync();
            var usersCondition = users.FirstOrDefault(u => u.Username == registerDTO.Username);
            if (usersCondition != null)
            {
                return null;
            }

            var role = await _roleRepository.GetByIdAsync(registerDTO.RoleId);
            if (role == null)
            {
                return null;
            }

            var user = _mapper.Map<User>(registerDTO);
            user.Password = _passwordHasher.HashPassword(user, registerDTO.Password);
            user.Role = role;

            await _user.AddAsync(user);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _user.DeleteAsync(id);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _user.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByNameAsync(string name)
        {
            var users = await _user.GetAllAsync();
            var userCondition = users.FirstOrDefault(u => u.Username == name);
            if (userCondition == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(userCondition);
        }

        public async Task<UserDTO> UpdateUserAsync(RegisterDTO registerDTO)
        {
            var data = _mapper.Map<User>(registerDTO);
            var user = _user.UpdateAsync(data);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
        {
            var users = await _user.GetAllAsync();
            foreach (var user in users)
            {
                var role = await _roleRepository.GetByIdAsync(user.RoleId);
                user.Role = role;
            }    
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
