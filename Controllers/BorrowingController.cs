using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services.BorrowingService;
using LibraryProject.Services.LibraryService;
using LibraryProject.Services.NotificationService;
using LibraryProject.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryProject.Controllers
{
    public class BorrowingController : Controller
	{
		private readonly IBorrowingService _borrowingService;
		private readonly ILibraryService _libraryService;
        private readonly IUserService _userService;
        private readonly INotificationObserver _notificationObserver;
        private readonly IRoleService _roleService;
        public BorrowingController(IBorrowingService borrowingService, ILibraryService libraryService, IUserService userService, INotificationObserver notificationObserver, IRoleService roleService)
		{
			_borrowingService = borrowingService;
			_libraryService = libraryService;
            _userService = userService;
		    _notificationObserver = notificationObserver;
            _roleService = roleService;
        }


		public async Task<IActionResult> Index()
		{
            var Role = HttpContext.Session.GetString("Role");
            IEnumerable<BorrowingDTO> borrowingsDTO;
            if (Role=="Reader")
            {
                var username = HttpContext.Session.GetString("Username");
                var userDTO = await _userService.GetUserByNameAsync(username);

                borrowingsDTO = await _borrowingService.GetBorrowingsByReaderAsync(userDTO.UserID);
            }    
            else
            {
                borrowingsDTO = await _borrowingService.GetAllBorrowingsAsync();
            }
			return View(borrowingsDTO);
		}

		//public async Task<IActionResult> Borrow()
		//{
		//	var books = await _libraryService.GetAllBooksAvailableAsync();
  //          ViewBag.Books = books.Select(b => new SelectListItem
  //          {
  //              Value = b.BookId.ToString(),
  //              Text = b.Title
  //          }).ToList();
		//	return View();
  //      }

        [HttpGet("Borrowing/Borrow/{bookId}")]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var Username = HttpContext.Session.GetString("Username");
            if (Username == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var book = await _libraryService.GetBookByIdAsync(bookId);
            if (book == null || !book.IsAvailable.Value)
            {
                TempData["Error"] = "This book is currently unavailable.";
                return RedirectToAction("Index", "Book");
            }

            var borrowDTO = new BorrowingDTO
            {
                BookID = book.BookId,
                BookTitle = book.Title,
                PricePerDay = book.BorrowingPrice,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
            };

            return View(borrowDTO);
        }

        [HttpPost]
		public async Task<IActionResult> Borrow(BorrowingDTO borrowingDTO)
		{
            var username = HttpContext.Session.GetString("Username");
            var userDTO = await _userService.GetUserByNameAsync(username);
            await _borrowingService.BorrowBookAsync(userDTO.UserID, borrowingDTO);

            var roles = await _roleService.GetAllRolesAsync();
            var admin = roles.FirstOrDefault(r => r.RoleName == "Admin");
            var librarian = roles.FirstOrDefault(r => r.RoleName == "Librarian");

            //await _notificationObserver.AddNotificationToBorrowBook(borrowingDTO, username, admin.RoleId);
            //await _notificationObserver.AddNotificationToBorrowBook(borrowingDTO, username, librarian.RoleId);

            var message = $"User {username} has borrowed the book '{borrowingDTO.BookTitle}'.";

            await _notificationObserver.SendNotificationToRoleAsync(message, admin.RoleId);
            await _notificationObserver.SendNotificationToRoleAsync(message, librarian.RoleId);

            return RedirectToAction("Index", new { userDTO.UserID });
		}

        [HttpGet("Borrowing/Return/{borrowingId}")]
        public async Task<IActionResult> Return(int borrowingId)
		{
			var borrowingDTO = await _borrowingService.GetBorrowingsByIdAsync(borrowingId);
            if (borrowingDTO == null)
            {
                TempData["Error"] = "Borrowing record not found.";
                return RedirectToAction("Index");
            }
            Console.WriteLine(borrowingDTO);

            DateTime borrowDate = borrowingDTO.BorrowDate.Date;
            DateTime returnDate = DateTime.Now;
            DateTime dueDate = borrowingDTO.DueDate.Date;

            int daysBorrowed = (returnDate.Date - borrowDate).Days;
            int daysOverdue = (returnDate > dueDate) ? (returnDate - dueDate).Days : 0;

            Console.WriteLine($"Days Borrowed: {daysBorrowed}");
            Console.WriteLine($"Days Overdue: {daysOverdue}");

            decimal pricePerDay = borrowingDTO.PricePerDay;
            decimal totalPrice = 0;
            decimal overdueFee = 0;

            if (daysBorrowed <= 10)
            {
                totalPrice = daysBorrowed * pricePerDay;
            }
            else if (daysBorrowed <= 20)
            {
                totalPrice = daysBorrowed * pricePerDay + ((daysBorrowed-10) * pricePerDay) * 1.25m;
            }
            else
            {
                totalPrice = daysBorrowed * pricePerDay + ((daysBorrowed - 10) * pricePerDay) * 1.25m + (daysBorrowed - 20) * pricePerDay * 1.5m;
            }

            Console.WriteLine(totalPrice);

            if (daysOverdue > 0)
            {
                overdueFee = totalPrice * 0.05m * daysOverdue;
            }

            Console.WriteLine(overdueFee);

            borrowingDTO.OverdueFee = overdueFee;
            borrowingDTO.BorrowPrice = totalPrice;
            borrowingDTO.ReturnDate = returnDate;
            borrowingDTO.OverdueDays = daysOverdue;
            borrowingDTO.PricePerDay = pricePerDay;

            Console.WriteLine(borrowingDTO);

            return View(borrowingDTO);
		}

        [HttpGet("Borrowing/Detail/{borrowingId}")]
        public async Task<IActionResult> Detail(int borrowingId)
        {
            var borrowing = await _borrowingService.GetBorrowingsByIdAsync(borrowingId);
            if (borrowing == null)
            {
                TempData["Error"] = "Borrowing record not found.";
                return RedirectToAction("Index");
            }

            return View(borrowing);
        }


        [HttpPost]
		public async Task<IActionResult> ReturnBook(BorrowingDTO borrowingDTO)
		{
            var borrowing = await _borrowingService.GetBorrowingsByIdAsync(borrowingDTO.BorrowingID);
            if (borrowing == null)
            {
                TempData["Error"] = "Borrowing record not found.";
                return RedirectToAction("Index");
            }

            var username = HttpContext.Session.GetString("Username");
            //var userDTO = await _userService.GetUserByNameAsync(username);

            borrowing.ReturnDate = borrowingDTO.ReturnDate;
            borrowing.BorrowPrice = borrowingDTO.BorrowPrice;
            borrowing.OverdueDays = borrowingDTO.OverdueDays;
            borrowing.OverdueFee = borrowingDTO.OverdueFee;
            borrowing.Notes = borrowingDTO.Notes;


            await _borrowingService.ReturnBookAsync(borrowing);

            var roles = await _roleService.GetAllRolesAsync();
            var admin = roles.FirstOrDefault(r => r.RoleName == "Admin");
            var librarian = roles.FirstOrDefault(r => r.RoleName == "Librarian");

            //await _notificationObserver.AddNotificationToReturnBook(borrowingDTO, username, admin.RoleId);
            //await _notificationObserver.AddNotificationToReturnBook(borrowingDTO, username, librarian.RoleId);

            var message = $"User {username} has returned the book '{borrowing.BookTitle}'.";

            await _notificationObserver.SendNotificationToRoleAsync(message, admin.RoleId);
            await _notificationObserver.SendNotificationToRoleAsync(message, librarian.RoleId);

            return RedirectToAction("Payment", "Payment", new { borrowingId = borrowingDTO.BorrowingID });
        }
	}
}
