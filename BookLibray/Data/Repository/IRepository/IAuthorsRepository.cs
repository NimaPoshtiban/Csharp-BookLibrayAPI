using BookLibrary.Models.Dtos.Author;

namespace BookLibrary.Data.Repository.IRepository
{
    public interface IAuthorsRepository : IRepository<Author>
    {
        Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id);
    }
}
