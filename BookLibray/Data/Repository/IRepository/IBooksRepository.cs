using BookLibrary.Models.Dtos.Book;

namespace BookLibrary.Data.Repository.IRepository
{
    public interface IBooksRepository : IRepository<Book>
    {
        Task<List<BookReadOnlyDto>> GetAllBooksAsync();
        Task<BookDetailsDto> GetBookAsync(int id);
    }
}
