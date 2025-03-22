using System.ComponentModel.DataAnnotations;

namespace LibraryProject.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
