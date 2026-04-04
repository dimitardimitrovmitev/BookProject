using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Controllers
{
    [Route("userbook")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly IUserBookRepository _userBookRepo;

        public UserBookController(IUserBookRepository userBookRepo)
        {
            _userBookRepo = userBookRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserBooks()
        {
            var userBooks = await _userBookRepo.GetAllUserBooksAsync();
            return Ok(userBooks.Select(ub => ub.ToReadDTO()));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBooksByUser([FromRoute] int userId)
        {
            var userBooks = await _userBookRepo.GetBooksByUserIdAsync(userId);

            if (userBooks == null || userBooks.Count == 0)
                return NotFound($"No books found for user ID {userId}.");

            return Ok(userBooks.Select(ub => ub.ToReadDTO()));
        }

        [HttpGet("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> GetUserBook([FromRoute] int userId, [FromRoute] int bookId)
        {
            var userBook = await _userBookRepo.GetUserBookAsync(userId, bookId);

            if (userBook == null) return NotFound();

            return Ok(userBook.ToReadDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AddBookToList([FromBody] UserBookCreateDTO dto)
        {
            var existing = await _userBookRepo.GetUserBookAsync(dto.UserId, dto.BookId);
            if (existing != null)
                return Conflict("This book is already in the user's reading list.");

            var userBook = dto.ToUserBookFromCreateDTO();
            var created = await _userBookRepo.AddUserBookAsync(userBook);

            return CreatedAtAction(nameof(GetUserBook),
                new { userId = created.UserId, bookId = created.BookId },
                created.ToReadDTO());
        }

        [HttpPut("user/{userId}/book/{bookId}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] int userId, [FromRoute] int bookId, [FromBody] UserBookMarkReadDTO dto)
        {
            var updated = await _userBookRepo.MarkAsReadAsync(userId, bookId, dto);

            if (updated == null) return NotFound();

            return Ok(updated.ToReadDTO());
        }

        [HttpPut("user/{userId}/book/{bookId}/rate")]
        public async Task<IActionResult> RateBook([FromRoute] int userId, [FromRoute] int bookId, [FromBody] UserBookRateDTO dto)
        {
            var updated = await _userBookRepo.RateBookAsync(userId, bookId, dto);

            if (updated == null) return NotFound();

            return Ok(updated.ToReadDTO());
        }

        [HttpDelete("user/{userId}/book/{bookId}")]
        public async Task<IActionResult> RemoveFromList([FromRoute] int userId, [FromRoute] int bookId)
        {
            var deleted = await _userBookRepo.RemoveUserBookAsync(userId, bookId);

            if (deleted == null) return NotFound();

            return NoContent();
        }
    }
}