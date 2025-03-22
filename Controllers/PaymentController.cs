using LibraryProject.DTOs;
using LibraryProject.Models;
using LibraryProject.Services.BorrowingService;
using LibraryProject.Services.PaymentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBorrowingService _borrowingService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IPaymentService _paymentService;
        public PaymentController(IBorrowingService borrowingService, IPaymentMethodService paymentMethodService, IPaymentService paymentService)
        {
            _borrowingService = borrowingService;
            _paymentMethodService = paymentMethodService;
            _paymentService = paymentService;
        }
        [Route("Payment/Payment/{borrowingId}")]
        public async Task<IActionResult> Payment(int borrowingId)
        {
            var borrowingDTO = await _borrowingService.GetBorrowingsByIdAsync(borrowingId);
            if (borrowingDTO == null) return RedirectToAction("Index");

            var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "MethodName");

            var Username = HttpContext.Session.GetString("Username");
            if (Username != borrowingDTO.ReaderName)
            {
                return RedirectToAction("Index", "Book");
            }

            var paymentDTO = new PaymentDTO
            {
                BorrowingID = borrowingDTO.BorrowingID,
                BookTitle = borrowingDTO.BookTitle,
                Amount = borrowingDTO.BorrowPrice.Value + borrowingDTO.OverdueFee
            };
            Console.WriteLine(paymentDTO);
            return View(paymentDTO);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(PaymentDTO paymentDTO)
        {
            
            var payment =  await _paymentService.AddPayment(new PaymentDTO
            {
                BorrowingID = paymentDTO.BorrowingID,
                Amount = paymentDTO.Amount,
                PaymentDate = DateTime.Now,
                PaymentMethodId = paymentDTO.PaymentMethodId,
                Notes = "Payment completed"
            });
            Console.WriteLine(payment);
            return RedirectToAction("Review", "Review", new { borrowingId = paymentDTO.BorrowingID });
        }

        [HttpGet]
        [Route("Payment/Detail/{borrowingId}")]
        public async Task<IActionResult> Detail(int borrowingId)
        {
            var payment = await _paymentService.GetPaymentByBorrowingIdAsync(borrowingId);
            if (payment == null)
            {
                TempData["Error"] = "No payment found for this borrowing.";
                return RedirectToAction("Index", "Borrowing");
            }

            return View(payment);
        }

    }
}
