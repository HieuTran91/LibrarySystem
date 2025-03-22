using LibraryProject.DTOs;

namespace LibraryProject.Services.ReviewService
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetAllReviewsOfBookAsync(int bookId);
        Task<ReviewDTO> GetReviewByIdAsync(ReviewDTO reviewDTO);
        Task<ReviewDTO> AddReview(ReviewDTO reviewDTO);
        Task<ReviewDTO> UpdateReview(ReviewDTO reviewDTO);
        Task DeleteReview(int reviewId);
        Task<IEnumerable<ReviewDTO>> GetAllReviewsAsync();
    }
}
