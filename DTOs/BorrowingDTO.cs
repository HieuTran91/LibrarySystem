namespace LibraryProject.DTOs
{
	public class BorrowingDTO
	{
		public int BorrowingID { get; set; }
		public int BookID { get; set; }
		public string BookTitle { get; set; }
        public string ReaderName { get; set; }
        public DateTime BorrowDate { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
        public decimal PricePerDay { get; set; }
        public decimal? BorrowPrice { get; set; }
        public int OverdueDays { get; set; }
        public decimal OverdueFee { get; set; }
        public string Notes { get; set; }
    }
}
