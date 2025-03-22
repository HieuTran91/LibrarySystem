using LibraryProject.DTOs;
using LibraryProject.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            //var IsLogin = _authService.AuthenticateAsync(loginDTO);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            
            var userDTO = await _authService.AuthenticateAsync(loginDTO);
            if (userDTO == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(loginDTO);
            }
            var accessToken = _tokenService.GenerateJwtToken(userDTO.Username, userDTO.RoleName);
            var refreshToken = await _tokenService.GenerateRefreshToken(userDTO.UserID, userDTO.RoleName);

            if (accessToken == null || refreshToken == null)
            {
                return BadRequest(new { message = "Failed to generate tokens." });
            }

            HttpContext.Session.SetString("AccessToken", accessToken);
            HttpContext.Session.SetString("RefreshToken", refreshToken.Token);
            HttpContext.Session.SetString("Username", userDTO.Username);
            HttpContext.Session.SetString("Role", userDTO.RoleName);

            Console.WriteLine("Saved Session: " + HttpContext.Session.GetString("AccessToken"));
            Console.WriteLine("Saved Session Claims: " + HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value);

            return RedirectToAction("Index", "Book");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.RoleId==null)
            {
                registerDTO.RoleId = 3;
            }
            if (registerDTO.Email == null)
            {
                registerDTO.Email = registerDTO.Username;
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDTO = await _authService.AddNewUser(registerDTO);
            Console.WriteLine("HELLO");
            if(userDTO==null)
            {
                ModelState.AddModelError("", "Account is exists.");
                return View();
            }    
            var accessToken = _tokenService.GenerateJwtToken(userDTO.Username, userDTO.RoleName);
            var refreshToken = await _tokenService.GenerateRefreshToken(userDTO.UserID, userDTO.RoleName);

            if (accessToken == null || refreshToken == null)
            {
                return BadRequest(new { message = "Failed to generate tokens." });
            }

            HttpContext.Session.SetString("AccessToken", accessToken);
            HttpContext.Session.SetString("RefreshToken", refreshToken.Token);
            HttpContext.Session.SetString("Username", userDTO.Username);
            HttpContext.Session.SetString("Role", userDTO.RoleName);

            return RedirectToAction("Index", "Book");
        }

        //[Authorize(Policy = "AllUsersTypePolicy")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = HttpContext.Session.GetString("RefreshToken");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token found in session");
            }

            // Tìm User có RefreshToken hợp lệ
            var user = await _tokenService.GetUserByRefreshToken(refreshToken);

            if (user == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            var oldRefreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && !rt.IsRevoked);
            oldRefreshToken.IsRevoked = true;

            await _tokenService.UpdateRefreshToken(oldRefreshToken);

            var newRefreshToken = await _tokenService.GenerateRefreshToken(user.UserId, user.Role.RoleName);
            var accessToken = _tokenService.GenerateJwtToken(user.Username, user.Role.RoleName);

            HttpContext.Session.SetString("RefreshToken", newRefreshToken.Token);

            return Ok(new { accessToken = accessToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
