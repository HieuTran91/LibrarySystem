using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Repositories.GenericRepository;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryProject.Services.LibraryService
{
    public class LibraryService : ILibraryService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Genre> _genreRepository;
        public LibraryService(IRepository<Book> bookRepository, IMapper mapper, IRepository<Genre> genreRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _genreRepository = genreRepository;
        }
        public async Task AddBookAsync(BookDTO bookDTO)
        {
            var book = _mapper.Map<Book>(bookDTO);
            var genre = await _genreRepository.GetByIdAsync((int)bookDTO.GenreId);
            book.Genre = genre;
            await _bookRepository.AddAsync(book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            //await _bookRepository.DeleteAsync(bookId);
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (!book.IsAvailable)
            {
                throw new InvalidOperationException("Cannot delete book that has not been returned.");
            }
            await _bookRepository.DeleteAsync(bookId);
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            foreach (var book in books)
            {
                var genre = await _genreRepository.GetByIdAsync((int)book.GenreId);
                book.Genre = genre;
            }
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooksAvailableAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            books = books.Where(book => book.IsAvailable);
            foreach(var book in books)
            {
                var genre = await _genreRepository.GetByIdAsync((int)book.GenreId);
                book.Genre = genre;
            }    
            return _mapper.Map<IEnumerable<BookDTO>>(books);
        }

        public async Task<BookDTO> GetBookByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            var genre = await _genreRepository.GetByIdAsync((int)book.GenreId);
            book.Genre = genre;
            return _mapper.Map<BookDTO>(book);
        }

        public async Task<IEnumerable<BookDTO>> SearchBooksAsync(string keyword)
        {
            var allBook = await _bookRepository.GetAllAsync();
            var conditionBooks = allBook.Where(b => b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            return _mapper.Map<IEnumerable<BookDTO>>(conditionBooks);
        }

        public async Task UpdateBookAsync(BookDTO bookDTO)
        {
            var book = _mapper.Map<Book>(bookDTO);
            await _bookRepository.UpdateAsync(book);
        }
    }
}
