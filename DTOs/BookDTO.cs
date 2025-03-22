using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace LibraryProject.DTOs
{
	public class BookDTO
	{
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;
        public decimal BorrowingPrice { get; set; }
        public decimal BookPrice { get; set; }

        public bool? IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public int? GenreId { get; set; }
        public string? GenreName { get; set; }
    }
}
