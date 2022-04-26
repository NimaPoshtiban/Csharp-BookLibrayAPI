using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookLibrary.Data.Repository.IRepository;
using BookLibrary.Models.Dtos.Author;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data.Repository
{
    public class AuthorsRepository : Repository<Author>, IAuthorsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthorsRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
        {
            return await context.Authors
                .Include(q => q.Books)
                .ProjectTo<AuthorDetailsDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
