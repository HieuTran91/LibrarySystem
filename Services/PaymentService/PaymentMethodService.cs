using AutoMapper;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;

namespace LibraryProject.Services.PaymentService
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        public PaymentMethodService(IRepository<PaymentMethod> paymentMethodRepository)
        { 
            _paymentMethodRepository = paymentMethodRepository;
        }
        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            return await _paymentMethodRepository.GetAllAsync();   
        }

        public async Task<PaymentMethod> GetPaymentMethodByIdAsync(int id)
        {
            return await _paymentMethodRepository.GetByIdAsync(id);
        }
    }
}
