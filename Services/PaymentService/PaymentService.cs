using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;

namespace LibraryProject.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<PaymentMethod> _paymentMethodRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Borrowing> _borrowingRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;
        public PaymentService(IRepository<Payment> paymentRepository, IMapper mapper, IRepository<Borrowing> borrowingRepository, IRepository<Book> bookRepository, IRepository<User> userRepository, IRepository<PaymentMethod> paymentMethodRepository)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<PaymentDTO> AddPayment(PaymentDTO paymentDTO)
        {
            var isPaymentExists = await IsPayment(paymentDTO.BorrowingID);
            if (isPaymentExists) {
                return null;
            }

            var borrowing = await _borrowingRepository.GetByIdAsync(paymentDTO.BorrowingID);
            if (borrowing == null)
            {
                throw new Exception("Borrowing không tồn tại");
            }

            var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
            if (book == null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(borrowing.UserId);
            if (user == null)
            {
                return null;
            }


            borrowing.Book = book;
            borrowing.User = user;


            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentDTO.PaymentMethodId);

            var payment = _mapper.Map<Payment>(paymentDTO);

            payment.BorrowingID = borrowing.BorrowingID;
            payment.Borrowing = borrowing;

            payment.PaymentMethodId = paymentMethod.PaymentMethodId;
            payment.PaymentMethod = paymentMethod;

            await _paymentRepository.AddAsync(payment);

            return _mapper.Map<PaymentDTO>(payment);
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            foreach (var payment in payments)
            {
                var borrowing = await _borrowingRepository.GetByIdAsync(payment.BorrowingID);
                var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
                var user = await _userRepository.GetByIdAsync(borrowing.UserId);
                borrowing.Book = book;
                borrowing.User = user;
            }
            return _mapper.Map<IEnumerable<PaymentDTO>>(payments);
        }

        public async Task<PaymentDTO> GetPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if(payment==null)
            {
                return null;
            }    
            var borrowing = await _borrowingRepository.GetByIdAsync(payment.BorrowingID);
            var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
            var user = await _userRepository.GetByIdAsync(borrowing.UserId);
            borrowing.Book = book;
            borrowing.User = user;
            return _mapper.Map<PaymentDTO>(payment);
        }

        public async Task<bool> IsPayment(int BorrowingID)
        {
            var payments = await _paymentRepository.GetAllAsync();
            var payment = payments.FirstOrDefault(p => p.BorrowingID == BorrowingID);
            if(payment==null)
            {
                return false;
            }    
            return true;
        }

        public async Task<PaymentDTO> GetPaymentByBorrowingIdAsync(int borrowingId)
        {
            var payments = await _paymentRepository.GetAllAsync();
            var payment = payments.FirstOrDefault(p => p.BorrowingID == borrowingId);
            if (payment == null)
            {
                return null;
            }

            var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId);
            if (borrowing == null)
            {
                return null;
            }

            var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
            if (book == null)
            {
                return null;
            }

            var user = await _userRepository.GetByIdAsync(borrowing.UserId);
            if (user == null)
            {
                return null;
            }


            borrowing.Book = book;
            borrowing.User = user;

            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(payment.PaymentMethodId);

            payment.BorrowingID = borrowing.BorrowingID;
            payment.Borrowing = borrowing;

            payment.PaymentMethodId = paymentMethod.PaymentMethodId;
            payment.PaymentMethod = paymentMethod;

            return _mapper.Map<PaymentDTO>(payment);
        }

    }
}
