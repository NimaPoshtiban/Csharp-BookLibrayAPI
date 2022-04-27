using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookLibrary.Data.Repository.IRepository;
using BookLibrary.Models.Dtos.Book;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data.Repository
{
    public class BooksRepository : Repository<Book>, IBooksRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BooksRepository(ApplicationDbContext context,IMapper mapper) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<BookReadOnlyDto>> GetAllBooksAsync()
        {
            return await context.Books.Include(a => a.Author)
                    .ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        public async Task<BookDetailsDto> GetBookAsync(int id)
        {
            return await context.Books
                ?.Include(a=>a.Author)
                ?.ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
                ?.FirstOrDefaultAsync(q=>q.Id== id); 
        }
    }
}
