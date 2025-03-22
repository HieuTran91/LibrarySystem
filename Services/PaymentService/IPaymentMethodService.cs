using LibraryProject.Models;

namespace LibraryProject.Services.PaymentService
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod> GetPaymentMethodByIdAsync(int id);
    }
}
