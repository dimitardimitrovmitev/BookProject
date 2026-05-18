using BookProject.Interfaces;
using BookProject.Mappers;
using BookProject.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("userbook")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly IUserBookRepository _userBookRepo;

        public UserBookController(IUserBookRepository userBookRepo)
        {
            _userBookRepo = userBookRepo;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserBooks([FromQuery] UserBookQueryObject query)
        {
            var result = await _userBookRepo.GetAllUserBooksAsync(query);

            return Ok(new
            {
                items = result.Items.Select(ub => ub.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyBooks([FromQuery] UserBookQueryObject query)
        {
            var result = await _userBookRepo.GetBooksByUserIdAsync(GetCurrentUserId(), query);
            if (result.TotalCount == 0)
                return NotFound("No books found.");
            return Ok(new
            {
                items = result.Items.Select(ub => ub.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetBooksByUser([FromRoute] int userId, [FromQuery] UserBookQueryObject query)
        {
            var result = await _userBookRepo.GetBooksByUserIdAsync(userId, query);

            if (result.TotalCount == 0)
                return NotFound($"No books found for user ID {userId}.");

            return Ok(new
            {
                items = result.Items.Select(ub => ub.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("user/{userId}/book/{bookId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserBook([FromRoute] int userId, [FromRoute] int bookId)
        {
            var userBook = await _userBookRepo.GetUserBookAsync(userId, bookId);
            if (userBook == null) return NotFound();
            return Ok(userBook.ToReadDTO());
        }

        [HttpPost]
        public async Task<IActionResult> AddBookToList([FromBody] UserBookCreateDTO dto)
        {
            var userId = GetCurrentUserId();
            var existing = await _userBookRepo.GetUserBookAsync(userId, dto.BookId);
            if (existing != null)
                return Conflict("This book is already in the user's reading list.");

            var userBook = dto.ToUserBookFromCreateDTO(userId);
            var created = await _userBookRepo.AddUserBookAsync(userBook);

            return CreatedAtAction(nameof(GetUserBook),
                new { userId = created.UserId, bookId = created.BookId },
                created.ToReadDTO());
        }

        [HttpPut("book/{bookId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int bookId, [FromBody] UserBookUpdateStatusDTO dto)
        {
            var updated = await _userBookRepo.UpdateStatusAsync(GetCurrentUserId(), bookId, dto);
            if (updated == null) return NotFound();
            return Ok(updated.ToReadDTO());
        }

        [HttpDelete("book/{bookId}")]
        public async Task<IActionResult> RemoveFromList([FromRoute] int bookId)
        {
            var userId = GetCurrentUserId();
            var deleted = await _userBookRepo.RemoveUserBookAsync(userId, bookId);
            if (deleted == null) return NotFound();
            return NoContent();
        }
    }
}