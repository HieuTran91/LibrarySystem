using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services.NotificationService;
using LibraryProject.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : Controller
    {
        private readonly INotificationObserver _notificationService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public NotificationController(INotificationObserver notificationService, IUserService userService, IRoleService roleService)
        {
            _notificationService = notificationService;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet("user-notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var username = HttpContext.Session.GetString("Username");
            var role = HttpContext.Session.GetString("Role");


            if (username == null) return Unauthorized();

            var roles = await _roleService.GetAllRolesAsync();
            var user = await _userService.GetUserByNameAsync(username);

            IEnumerable<NotificationDTO> notifications;

            if(role=="Admin")
            {
                var admin = roles.FirstOrDefault(r => r.RoleName == "Admin");
                notifications = await _notificationService.GetNotificationsForRole(admin.RoleId);
            }
            else if(role=="Librarian")
            {
                var librarian = roles.FirstOrDefault(r => r.RoleName == "Librarian");
                notifications = await _notificationService.GetNotificationsForRole(librarian.RoleId);
            }
            else
            {
                notifications = await _notificationService.GetNotificationsForUser(user.UserID);
            }

            if (notifications == null || !notifications.Any())
            {
                return Ok(new List<NotificationDTO>());
            }

            return Ok(notifications.Select(n => new NotificationDTO
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            }));
        }

        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.UpdateNotificationReaded(id);
            return Ok();
        }
    }
}
