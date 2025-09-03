using BookProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{
    [Route("character")]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CharacterController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCharacters()
        {
            var characters = _context.Characters.ToList();
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public IActionResult GetCharacter([FromRoute] int id)
        {
            var character = _context.Characters.Find(id);
            if (character == null)
            {
                return NotFound();
            }
            return Ok(character);
        }
    }
}