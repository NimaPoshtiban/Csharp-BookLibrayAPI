#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Data;
using BookLibrary.Models.Dtos.Author;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BookLibrary.Data.Repository.IRepository;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            try
            {
                var authors = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _unitOfWork.Authors.GetAllAsync());
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retriving authors from db at {nameof(GetAuthors)}");
                return StatusCode(500);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _unitOfWork.Authors.GetAuthorDetailsAsync(id);/* _mapper.Map<AuthorReadOnlyDto>(await _unitOfWork.Authors.GetAsync(id));*/

                if (author == null)
                {
                    return NotFound();
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could'nt get author from db at {nameof(GetAuthor)}");
                return StatusCode(500);
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest();
            }

            var author = await _unitOfWork.Authors.GetAsync(authorDto.Id);
            if (author == null)
            {
                return NotFound();
            }

            _mapper.Map(authorDto, author);

            try
            {
                await _unitOfWork.Authors.UpdateAsync(author);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _unitOfWork.Authors.AddAsync(author);
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error happend during author creation at {nameof(PostAuthor)}");
                return StatusCode(500);
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {

            try
            {
                var author = await _unitOfWork.Authors.GetAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                await _unitOfWork.Authors.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Could'nt delete the author at {nameof(DeleteAuthor)}");
                return StatusCode(500);
            }
            return NoContent();
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _unitOfWork.Authors.Exists(id);
        }
    }
}
