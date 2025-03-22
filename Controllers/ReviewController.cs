using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services.BorrowingService;
using LibraryProject.Services.ReviewService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IBorrowingService _borrowingService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IBorrowingService borrowingService, IMapper mapper)
        {
            _reviewService = reviewService;
            _borrowingService = borrowingService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Review/Review/{borrowingId}")]
        public async Task<IActionResult> Review(int borrowingId)
        {

            var borrowingDTO = await _borrowingService.GetBorrowingsByIdAsync(borrowingId);
            if (borrowingDTO == null || borrowingDTO.ReturnDate == null)
            {
                TempData["Error"] = "You must return the book before reviewing it.";
                return RedirectToAction("Index", "Book");
            }

            var Username = HttpContext.Session.GetString("Username");
            if(Username != borrowingDTO.ReaderName)
            {
                return RedirectToAction("Index", "Book");
            }

            var reviewDTO = new ReviewDTO
            {
                Borrowing = _mapper.Map<Borrowing>(borrowingDTO),
                BookTitle = borrowingDTO.BookTitle,
                BookId = borrowingDTO.BookID,
                Username = borrowingDTO.ReaderName
            };

            return View(reviewDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewDTO reviewDTO)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid review submission.";
                return RedirectToAction("Review", new { borrowingId = reviewDTO.Borrowing.BorrowingID });
            }

            var borrowingDTO = await _borrowingService.GetBorrowingsByIdAsync(reviewDTO.Borrowing.BorrowingID);
            if (borrowingDTO == null || borrowingDTO.ReturnDate == null)
            {
                TempData["Error"] = "You must return the book before reviewing it.";
                return RedirectToAction("Index", "Book");
            }

            var review = new ReviewDTO
            {
                Borrowing = _mapper.Map<Borrowing>(borrowingDTO),
                BookId = reviewDTO.BookId,
                Username = reviewDTO.Username,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CreatedAt = DateTime.Now
            };

            await _reviewService.AddReview(review);

            TempData["Success"] = "Review submitted successfully.";
            return RedirectToAction("Detail", "Book", new { id = reviewDTO.BookId });
        }

        public IActionResult SkipReview()
        {
            return RedirectToAction("Index", "Borrowing");
        }
    }
}
