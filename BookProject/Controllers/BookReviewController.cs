using BookProject.Interfaces;
using BookProject.Mappers;
using BookProject.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.BookReviewDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("bookreview")]
    [ApiController]
    public class BookReviewController : ControllerBase
    {
        private readonly IBookReviewRepository _reviewRepo;

        public BookReviewController(IBookReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] BookReviewQueryObject query)
        {
            var result = await _reviewRepo.GetAllReviewsAsync(query);

            return Ok(new
            {
                items = result.Items.Select(r => r.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview([FromRoute] int id)
        {
            var review = await _reviewRepo.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review.ToReadDTO());
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetReviewsByBook([FromRoute] int bookId)
        {
            var reviews = await _reviewRepo.GetReviewsByBookIdAsync(bookId);
            if (reviews == null || reviews.Count == 0)
                return NotFound($"No reviews found for book ID {bookId}.");
            return Ok(reviews.Select(r => r.ToReadDTO()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] BookReviewCreateDTO dto)
        {
            var reviewModel = dto.ToBookReviewFromCreateDTO(GetCurrentUserId());
            var created = await _reviewRepo.CreateReviewAsync(reviewModel);
            return CreatedAtAction(nameof(GetReview), new { id = created.BookReviewId }, created.ToReadDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, [FromBody] BookReviewUpdateDTO dto)
        {
            var updated = await _reviewRepo.UpdateReviewAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated.ToReadDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] int id)
        {
            var deleted = await _reviewRepo.DeleteReviewAsync(id);
            if (deleted == null) return NotFound();
            return NoContent();
        }
    }
}