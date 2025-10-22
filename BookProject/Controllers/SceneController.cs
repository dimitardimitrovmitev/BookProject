using BookProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [Route("scene")]
    [ApiController]
    public class SceneController : ControllerBase
    {
        private readonly ApplicationDBContext _context;


        public SceneController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetScenes()
        {
            var scenes = _context.Scenes.ToList();
            return Ok(scenes);
        }
        [HttpGet("{id}")]
        public IActionResult GetScene([FromRoute] int id)
        {
            var scene = _context.Scenes.Find(id);
            if (scene == null)
            {
                return NotFound();
            }
            return Ok(scene);
        }
    }
}
