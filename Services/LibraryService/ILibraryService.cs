using LibraryProject.DTOs;
using LibraryProject.Models;

namespace LibraryProject.Services.LibraryService
{
    public interface ILibraryService
    {
        Task<IEnumerable<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int bookId);
        Task AddBookAsync(BookDTO bookDTO);
        Task UpdateBookAsync(BookDTO bookDTO);
        Task DeleteBookAsync(int bookId);
        Task<IEnumerable<BookDTO>> SearchBooksAsync(string keyword);

        Task<IEnumerable<BookDTO>> GetAllBooksAvailableAsync();
    }
}
