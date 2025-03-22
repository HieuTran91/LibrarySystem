using System.ComponentModel.DataAnnotations;

namespace LibraryProject.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public int RoleId { get; set; } = 3;
    }
}
