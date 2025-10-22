using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.BookDTOs;
using static BookProject.Mappers.BookMappers;
namespace BookProject.Controllers
{

    [Route("book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;
        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
           
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookRepo.GetAllBooksAsync();

            var bookDto = books.Select(s => s.ToReadDTO());

            return Ok(bookDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);

            if (book == null) return NotFound();

            return Ok(book.ToReadDTO());
        }

        [HttpPost("manual")]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDTO bookDto)
        {
            var bookModel = bookDto.ToBookFromCreateDTO();

            await _bookRepo.AddBookAsync(bookModel);

            return CreatedAtAction(nameof(GetBookById), new { id = bookModel.Id }, bookModel.ToReadDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookUpdateDTO bookDto)
        {
            var bookModel = await _bookRepo.UpdateBookAsync(id, bookDto);

            if (bookModel is null) return NotFound();

            return Ok(bookModel.ToReadDTO());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var bookModel = await _bookRepo.DeleteBookAsync(id);

            if (bookModel is null) return NotFound();

            return NoContent();
        }
    }
}