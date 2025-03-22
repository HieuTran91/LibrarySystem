using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using System.Net;

namespace LibraryProject.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Borrowing> _borrowingRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<User> _userRepository;
        public ReviewService(IRepository<Review> reviewRepository, IMapper mapper, IRepository<Borrowing> borrowingRepository, IRepository<Book> bookRepository, IRepository<User> userRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task<ReviewDTO> AddReview(ReviewDTO reviewDTO)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewDTO.ReviewId);
            if (review != null) { return null; }
            review = _mapper.Map<Review>(reviewDTO);
            var borrowing = await _borrowingRepository.GetByIdAsync(review.BorrowingId);
            var book = await _bookRepository.GetByIdAsync(review.BookId);
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == reviewDTO.Username);
            review.User = user;
            review.Borrowing = borrowing;
            review.Book = book;
            review.UserId = user.UserId;

            await _reviewRepository.AddAsync(review);
            return _mapper.Map<ReviewDTO>(review);
        }

        public Task DeleteReview(int reviewId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            foreach(var review in reviews)
            {
                var borrowing = await _borrowingRepository.GetByIdAsync(review.BorrowingId);
                var book = await _bookRepository.GetByIdAsync(review.BookId);
                var user = await _userRepository.GetByIdAsync(review.UserId);
                review.User = user;
                review.Borrowing = borrowing;
                review.Book = book;
            }
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsOfBookAsync(int bookId)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            var reviewsOfBook = reviews.Where(r => r.BookId == bookId).ToList();
            foreach (var review in reviewsOfBook)
            {
                var borrowing = await _borrowingRepository.GetByIdAsync(review.BorrowingId);
                var book = await _bookRepository.GetByIdAsync(review.BookId);
                var user = await _userRepository.GetByIdAsync(review.UserId);
                review.User = user;
                review.Borrowing = borrowing;
                review.Book = book;
            }
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> GetReviewByIdAsync(ReviewDTO reviewDTO)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewDTO.ReviewId);
            var borrowing = await _borrowingRepository.GetByIdAsync(review.BorrowingId);
            var book = await _bookRepository.GetByIdAsync(review.BookId);
            var user = await _userRepository.GetByIdAsync(review.UserId);
            review.User = user;
            review.Borrowing = borrowing;
            review.Book = book;
            return _mapper.Map<ReviewDTO>(review);
        }

        public Task<ReviewDTO> UpdateReview(ReviewDTO reviewDTO)
        {
            throw new NotImplementedException();
        }
    }
}
