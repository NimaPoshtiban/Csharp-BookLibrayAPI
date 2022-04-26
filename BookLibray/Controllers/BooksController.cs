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
using Microsoft.AspNetCore.Authorization;
using BookLibrary.Data.Repository.IRepository;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BooksController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                var books = await _unitOfWork.Books.GetAllBooksAsync();

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
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            try
            {
                if (!await BookExists(id))
                {
                    return BadRequest();
                }

                var book = await _unitOfWork.Books.GetBookAsync(id);

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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await _unitOfWork.Books.GetAsync(bookDto.Id);

            if (book == null)
            {
                return BadRequest();
            }
            _mapper.Map(bookDto, book);


            try
            {
                await _unitOfWork.Books.UpdateAsync(book);
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
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return BadRequest();
                }

                var book = _mapper.Map<Book>(bookDto);

                await _unitOfWork.Books.AddAsync(book);


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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _unitOfWork.Books.GetAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Books.DeleteAsync(id);

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
            return await _unitOfWork.Books.Exists(id);
        }
    }
}
