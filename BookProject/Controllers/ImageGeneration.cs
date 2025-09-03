using BookProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [Route("imagegeneration")]
    public class ImageGeneration : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ImageGeneration(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGenerations()
        {
            var generations = _context.ImageGenerations.ToList();
            return Ok(generations);
        }
        [HttpGet("{id}")]
        public IActionResult GetGeneration([FromRoute] int id)
        {
            var generation = _context.ImageGenerations.Find(id);
            if (generation == null)
            {
                return NotFound();
            }
            return Ok(generation);
        }
    }
}
