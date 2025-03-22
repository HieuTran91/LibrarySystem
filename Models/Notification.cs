namespace LibraryProject.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        public int? RecipientRoleId { get; set; }
        public Role? RecipientRole { get; set; }

        public int? RecipientUserId { get; set; }
        public User? RecipientUser { get; set; }
    }

}
