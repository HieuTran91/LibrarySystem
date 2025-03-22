using LibraryProject.Models;

namespace LibraryProject.DTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int BookId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Borrowing? Borrowing { get; set; }
        public string BookTitle { get; set; }
        public string Username { get; set; }
    }
}
