namespace LibraryProject.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public decimal BookPrice { get; set; }
        public decimal BorrowingPrice { get; set; }
    }
}
