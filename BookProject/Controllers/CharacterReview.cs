using BookProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [Route("characterreview")]
    public class CharacterReview : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CharacterReview(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetReviews()
        {
            var reviews = _context.CharacterReviews.ToList();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        
        public IActionResult GetReview([FromRoute] int id)
        {
            var review = _context.CharacterReviews.Find(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }
    }
}
