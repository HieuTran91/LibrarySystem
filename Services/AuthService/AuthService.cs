using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using LibraryProject.Repositories.LoginRepository;
using LibraryProject.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;

namespace LibraryProject.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<object> _passwordHasher;
        private readonly IMapper _mapper;
        public AuthService(IAuthRepository authRepository, IRepository<User> userRepository, IRepository<Role> roleRepository, IPasswordHasher<object> passwordHasher, IMapper mapper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> AddNewUser(RegisterDTO registerDTO)
        {
            var users = await _userRepository.GetAllAsync();
            var usersCondition = users.Any(u => u.Username == registerDTO.Username);
            if (usersCondition)
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

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(user);
        }


        public async Task<UserDTO> AuthenticateAsync(LoginDTO loginDTO)
        {
            var user = await _authRepository.GetLoginAsync(loginDTO.Username);
            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDTO.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return _mapper.Map<UserDTO>(user);
        }
    }
}
