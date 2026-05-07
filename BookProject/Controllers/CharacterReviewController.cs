using BookProject.Interfaces;
using BookProject.Mappers;
using BookProject.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("characterreview")]
    [ApiController]
    public class CharacterReviewController : ControllerBase
    {
        private readonly ICharacterReviewRepository _reviewRepo;

        public CharacterReviewController(ICharacterReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        public async Task<IActionResult> GetReviews([FromQuery] CharacterReviewQueryObject query)
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

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CharacterReviewCreateDTO dto)
        {
            var reviewModel = dto.ToCharacterReviewFromCreateDTO(GetCurrentUserId());
            var created = await _reviewRepo.CreateReviewAsync(reviewModel);
            return CreatedAtAction(nameof(GetReview), new { id = created.CharacterReviewId }, created.ToReadDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, [FromBody] CharacterReviewUpdateDTO dto)
        {
            var updated = await _reviewRepo.UpdateReviewAsync(id, GetCurrentUserId(), dto);
            if (updated == null) return NotFound();
            return Ok(updated.ToReadDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] int id)
        {
            var deleted = await _reviewRepo.DeleteReviewAsync(id, GetCurrentUserId());
            if (deleted == null) return NotFound();
            return NoContent();
        }
    }
}
