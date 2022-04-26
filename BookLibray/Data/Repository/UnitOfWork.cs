using AutoMapper;
using BookLibrary.Data.Repository.IRepository;

namespace BookLibrary.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            Authors = new AuthorsRepository(_context);
            Books = new BooksRepository(_context,_mapper);
        }
        public IAuthorsRepository Authors { get;private set; }
        public IBooksRepository Books { get;private set; }
    }
}
