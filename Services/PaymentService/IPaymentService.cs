using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetAllPaymentAsync();
        Task<PaymentDTO> GetPaymentByIdAsync(int id);
        Task<PaymentDTO> AddPayment(PaymentDTO paymentDTO);
        Task<bool> IsPayment(int BorrowingID);
        Task<PaymentDTO> GetPaymentByBorrowingIdAsync(int borrowingId);
    }
}
