using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services.FirebaseService;
using LibraryProject.Services.LibraryService;
using LibraryProject.Services.ReviewService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryProject.Controllers
{
    public class BookController : Controller
	{
		private readonly ILibraryService _libraryService;
        private readonly IGenreService _genreService;
        private readonly IReviewService _reviewService;
        public BookController(ILibraryService libraryService, IGenreService genreService, IReviewService reviewService)
		{
			_libraryService = libraryService;
            _reviewService = reviewService;
            _genreService = genreService;
		}

        //[Authorize]
        public async Task<IActionResult> Index(int? genreId, string searchQuery)
		{
            var booksDTO = await _libraryService.GetAllBooksAsync();
            var genres = await _genreService.GetAllGenresAsync();

            ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
            ViewBag.SearchQuery = searchQuery;

            var books = genreId.HasValue
                ? booksDTO.Where(b => b.GenreId == genreId)
                : booksDTO;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                //string lowerSearchQuery = searchQuery.ToLower();
                books = books.Where(b =>
                    b.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.ToLower().Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            return View(books.ToList());    
        }


        //[Authorize(Policy = "AllUsersTypePolicy")]
        //[Authorize]
        public async Task<IActionResult> Detail(int id)
        {
            var bookDTO = await _libraryService.GetBookByIdAsync(id);
            var reviews = await _reviewService.GetAllReviewsOfBookAsync(id);
            ViewBag.Reviews = reviews;

            return View(bookDTO);
        }


        //[Authorize(Policy = "AdminLibrarianPolicy")]
        public async Task<IActionResult> Create()
        {
            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = new SelectList(genres, "GenreId", "GenreName");
            return View();
        }

        //[Authorize]
        [HttpPost]
		public async Task<IActionResult> Create(BookDTO bookDTO, IFormFile imageFile, [FromServices] FirebaseStorageService firebaseStorageService)
		{
            bookDTO.IsAvailable = true;

            if (ModelState.IsValid)
			{
                if (imageFile != null)
                {
                    string imageUrl = await firebaseStorageService.UploadImageAsync(imageFile);
                    bookDTO.ImageUrl = imageUrl;
                }
                await _libraryService.AddBookAsync(bookDTO);
				return RedirectToAction(nameof(Index));
			}
			return View(bookDTO);
		}

        //[Authorize]
        //[Authorize(Policy = "AdminLibrarianPolicy")]
        public async Task<IActionResult> Edit(int id)
		{
			var bookDTO = await _libraryService.GetBookByIdAsync(id);
			if (bookDTO == null) return NotFound();
			return View(bookDTO);
		}

        //[Authorize]
        //[Authorize(Policy = "AdminLibrarianPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(BookDTO bookDTO, IFormFile imageFile, [FromServices] FirebaseStorageService firebaseStorageService)
        {
            if (ModelState.IsValid)
            {
                var existingBook = await _libraryService.GetBookByIdAsync(bookDTO.BookId);
                if (existingBook == null)
                {
                    return NotFound();
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Upload ảnh mới lên Firebase Storage
                    string newImageUrl = await firebaseStorageService.UploadImageAsync(imageFile);

                    if (!string.IsNullOrEmpty(existingBook.ImageUrl))
                    {
                        await firebaseStorageService.DeleteImageAsync(existingBook.ImageUrl);
                    }

                    bookDTO.ImageUrl = newImageUrl;
                }
                else
                {
                    bookDTO.ImageUrl = existingBook.ImageUrl;
                }

                await _libraryService.UpdateBookAsync(bookDTO);

                return RedirectToAction(nameof(Index));
            }

            return View(bookDTO);
        }


        //[Authorize]
        //[Authorize(Policy = "AdminLibrarianPolicy")]
        public async Task<IActionResult> Delete(int id)
		{
			await _libraryService.DeleteBookAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
