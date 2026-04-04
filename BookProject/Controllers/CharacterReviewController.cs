using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Controllers
{
    [Route("characterreview")]
    [ApiController]
    public class CharacterReviewController : ControllerBase
    {
        private readonly ICharacterReviewRepository _reviewRepo;

        public CharacterReviewController(ICharacterReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewRepo.GetAllReviewsAsync();
            return Ok(reviews.Select(r => r.ToReadDTO()));
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
            var reviewModel = dto.ToCharacterReviewFromCreateDTO();
            var created = await _reviewRepo.CreateReviewAsync(reviewModel);
            return CreatedAtAction(nameof(GetReview), new { id = created.CharacterReviewId }, created.ToReadDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, [FromBody] CharacterReviewUpdateDTO dto)
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