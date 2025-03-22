using LibraryProject.Models;

namespace LibraryProject.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        public int? RecipientRoleId { get; set; }
        public string? RecipientRoleName { get; set; }

        public int? RecipientUserId { get; set; }
        public string? RecipientUserName { get; set; }
    }
}
