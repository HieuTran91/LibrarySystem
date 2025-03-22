namespace LibraryProject.DTOs
{
	public class UserDTO
	{
		public int UserID { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
