using BookLibrary.Data.Repository.IRepository;

namespace BookLibrary.Data.Repository
{
    public class AuthorsRepository : Repository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
