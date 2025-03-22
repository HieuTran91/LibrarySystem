using LibraryProject.Models;

namespace LibraryProject.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int BorrowingID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public int PaymentMethodId { get; set; }
        public string Notes { get; set; }
        public Borrowing Borrowing { get; set; }
        public string PaymentMethod { get; set; }
        public string BookTitle { get; set; }
        public string Username { get; set; }
    }
}
