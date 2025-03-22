using AutoMapper;
using LibraryProject.DTOs;
using LibraryProject.Services.LibraryService;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IMapper _mapper;
        public HomeController(ILibraryService libraryService, IMapper mapper)
        {
            _libraryService = libraryService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _libraryService.GetAllBooksAsync(); ;
            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(books);
            return View(booksDTO);
        }
    }
}
