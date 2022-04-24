#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Data;
using AutoMapper;
using BookLibrary.Models.Dtos.Book;
using AutoMapper.QueryableExtensions;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ApplicationDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                var books = await _context.Books.Include(a => a.Author)
                    .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error happend  at ${nameof(GetBooks)}");
                return StatusCode(500);
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookReadOnlyDto>> GetBook(int id)
        {
            try
            {
                if (!await BookExists(id))
                {
                    return BadRequest();
                }

                var book = await _context.Books
                    .Include(a => a.Author)
                    .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, $"Error happend at ${nameof(GetBook)}");
                return StatusCode(500);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(bookDto.Id);

            if (book == null)
            {
                return BadRequest();
            }
            _mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500);
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return BadRequest();
                }

                var book = _mapper.Map<Book>(bookDto);

                await _context.Books.AddAsync(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error happend during post operation at {nameof(PostBook)}");
                return StatusCode(500);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error happend during delete operation at {nameof(DeleteBook)}");
                return StatusCode(500);
            }
        }

        private async Task<bool> BookExists(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
