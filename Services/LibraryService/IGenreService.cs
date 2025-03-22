using LibraryProject.Models;

namespace LibraryProject.Services.LibraryService
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(int id);
    }
}
