namespace BookLibrary.Models.Dtos.Book
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Isbn { get; set; } = null!;
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}