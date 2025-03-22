using OrderApiProject_week2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        // Liên kết với Role
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public string? MembershipNumber { get; set; }
        public string? EmployeeNumber { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
