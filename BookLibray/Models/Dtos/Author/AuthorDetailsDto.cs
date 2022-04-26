using BookLibrary.Models.Dtos.Book;

namespace BookLibrary.Models.Dtos.Author
{
    public class AuthorDetailsDto : AuthorReadOnlyDto
    {
        public List<BookReadOnlyDto> Books { get; set; }
    }
}
