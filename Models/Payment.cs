using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryProject.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BorrowingID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public int PaymentMethodId { get; set; }
        public string Notes { get; set; }
        public Borrowing? Borrowing { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
