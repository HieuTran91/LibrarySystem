using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.BorrowingService
{
    public interface IBorrowingService
    {
        Task<BorrowingDTO> BorrowBookAsync(int userId, BorrowingDTO borrowingDTO);
        Task<BorrowingDTO> ReturnBookAsync(BorrowingDTO borrowingDTO);
        Task<IEnumerable<BorrowingDTO>> GetBorrowingsByReaderAsync(int userId);
        Task<IEnumerable<BorrowingDTO>> GetAllBorrowingsAsync();
        Task<BorrowingDTO> GetBorrowingsByIdAsync(int Id);
    }
}
