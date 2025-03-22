using LibraryProject.Models;

namespace OrderApiProject_week2.Models
{
    public class RefreshToken
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public User? User { get; set; }
    }

}
