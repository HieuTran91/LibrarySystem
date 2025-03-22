using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;

namespace LibraryProject.Services.LibraryService
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepository;
        public GenreService(IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }
        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _genreRepository.GetByIdAsync(id);
        }
    }
}
