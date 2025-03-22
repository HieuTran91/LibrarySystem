namespace LibraryProject.Models
{
	public class Borrowing
	{
		public int BorrowingID { get; set; }
		public int BookID { get; set; }
        public Book? Book { get; set; }
        public Payment? Payment { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime BorrowDate { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
        public decimal? BorrowPrice { get; set; }
        public int? OverdueDays { get; set; } 
        public decimal? OverdueFee { get; set; }
        public string? Notes { get; set; }
        public Review? Review { get; set; }
    }
}
