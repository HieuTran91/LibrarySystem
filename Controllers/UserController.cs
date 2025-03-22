using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;

namespace LibraryProject.Controllers
{
	public class UserController : Controller
	{
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
		public async Task<IActionResult> Index()
		{
            var userRoleFromSession = HttpContext.Session.GetString("Role");
            Console.WriteLine($"User Role from Session: {userRoleFromSession}");

            if (string.IsNullOrEmpty(userRoleFromSession))
            {
                Console.WriteLine("User has no role, redirecting to login...");
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                
                var users = await _userService.GetAllUserAsync();
                return View(users);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> Detail(int id)
		{
            var userDTO = await _userService.GetUserByIdAsync(id);
            return View(userDTO);
        }

        public async Task<IActionResult> Information()
        {
            var Username = HttpContext.Session.GetString("Username");
            var userDTO = await _userService.GetUserByNameAsync(Username);
            if(userDTO==null)
            {
                throw new Exception("User is not exists.");
            }    
            return View(userDTO);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO registerDTO)
		{
            if (ModelState.IsValid)
            {
                await _userService.AddUserAsync(registerDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(registerDTO);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userDTO = await _userService.GetUserByIdAsync(id);
            if (userDTO == null) return NotFound();
            return View(userDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO userDTO)
        {
            var userRoleFromSession = HttpContext.Session.GetString("Role");
            var registerDTO = _mapper.Map<RegisterDTO>(userDTO);
            if(registerDTO.RoleId==null)
            {
                registerDTO.RoleId = 3;
            }    
            await _userService.UpdateUserAsync(registerDTO);
            if (userRoleFromSession=="Admin" || userRoleFromSession== "Libraraian")
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Information", "User");
            //return View(userDTO);
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
