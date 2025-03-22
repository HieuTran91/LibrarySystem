using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Services.BorrowingService
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IRepository<Borrowing> _borrowingRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public BorrowingService(IRepository<Borrowing> borrowingRepository, IRepository<Book> bookRepository, IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        public async Task<BorrowingDTO> BorrowBookAsync(int userId, BorrowingDTO borrowingDTO)
        {
            var book = await _bookRepository.GetByIdAsync(borrowingDTO.BookID);
            Console.WriteLine(book);
            if (!book.IsAvailable)
            {
                return null;
            }


            var borrowing = new Borrowing
            {
                UserId = userId,
                BookID = borrowingDTO.BookID,
                BorrowDate = DateTime.Now,
                DueDate = borrowingDTO.DueDate
            };

            await _borrowingRepository.AddAsync(borrowing);            

            book.IsAvailable = false;
            await _bookRepository.UpdateAsync(book);
            
            return _mapper.Map<BorrowingDTO>(borrowing);
        }

        public async Task<IEnumerable<BorrowingDTO>> GetAllBorrowingsAsync()
        {
            var borrowings = await _borrowingRepository.GetAllAsync();
            foreach (var borrowing in borrowings)
            {
                var user = await _userRepository.GetByIdAsync(borrowing.UserId);
                borrowing.User = user;
                var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
                borrowing.Book = book;
            }
            return _mapper.Map<IEnumerable<BorrowingDTO>>(borrowings);
        }

        public async Task<BorrowingDTO> GetBorrowingsByIdAsync(int Id)
        {
            Console.WriteLine(Id);
            var allBorrowings = await _borrowingRepository.GetAllAsync();
            var borrowing = allBorrowings.FirstOrDefault(b => b.BorrowingID == Id);
            var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
            borrowing.Book = book;
            var user = await _userRepository.GetByIdAsync(borrowing.UserId);
            borrowing.User = user;

            return _mapper.Map<BorrowingDTO>(borrowing);
        }

        public async Task<IEnumerable<BorrowingDTO>> GetBorrowingsByReaderAsync(int userId)
        {
            var allBorrowings = await _borrowingRepository.GetAllAsync();
            var conditionBorrowings = allBorrowings.Where(b => b.UserId == userId);
            foreach(var borrowing in conditionBorrowings)
            {
                var book = await _bookRepository.GetByIdAsync(borrowing.BookID);
                borrowing.Book = book;
                var user = await _userRepository.GetByIdAsync(borrowing.UserId);
                borrowing.User = user;
            }    
            return _mapper.Map<IEnumerable<BorrowingDTO>>(conditionBorrowings);
        }

        public async Task<BorrowingDTO> ReturnBookAsync(BorrowingDTO borrowingDTO)
        {
            var book = await _bookRepository.GetByIdAsync(borrowingDTO.BookID);
            book.IsAvailable = true;

            var borrowingBook = await _borrowingRepository.GetByIdAsync(borrowingDTO.BorrowingID);
            if (borrowingBook == null)
            {
                throw new Exception("Borrowing record not found.");
            }


            borrowingBook.ReturnDate = borrowingDTO.ReturnDate;
            borrowingBook.BorrowPrice = borrowingDTO.BorrowPrice;
            borrowingBook.OverdueDays = borrowingDTO.OverdueDays;
            borrowingBook.OverdueFee = borrowingDTO.OverdueFee;
            borrowingBook.Notes = borrowingDTO.Notes;


            //var existingBorrowing = _context.Borrowings.Local.FirstOrDefault(b => b.BorrowingID == borrowingDTO.BorrowingID);
            //if (existingBorrowing != null)
            //{
            //    _context.Entry(existingBorrowing).State = EntityState.Detached; 
            //}

            _borrowingRepository.Attach(borrowingBook);

            await _bookRepository.UpdateAsync(book);

            return _mapper.Map<BorrowingDTO>(borrowingBook);
        }
    }
}
