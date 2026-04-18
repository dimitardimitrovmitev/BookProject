using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.BookDTOs;
using static BookProject.DTOs.CharacterDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("character")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        private readonly ICharacterRepository _characterRepo;
        public CharacterController(ApplicationDBContext context, ICharacterRepository characterRepo)
        {
            _context = context;
            _characterRepo = characterRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCharactersAsync()
        {
            var characters = await _characterRepo.GetAllCharactersAsync();
            return Ok(characters.Select(c => c.ToReadDTO()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterByIdAsync([FromRoute] int id)
        {
            var character = await _characterRepo.GetCharacterByIdAsync(id);
            if (character == null) return NotFound();
            
            return Ok(character.ToReadDTO());
        }

        [HttpPost("manual")]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateDTO characterDto)
        {
            var characterModel = characterDto.ToCharacterFromCreateDTO();

            await _characterRepo.AddCharacterAsync(characterModel);

            // The route value must match the parameter name in GetCharacterByIdAsync, which is "id"
            return CreatedAtAction("GetCharacterById", new { id = characterModel.CharacterId }, characterModel.ToReadDTO());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteCharacter([FromRoute] int id)
        {
            var characterModel = await _characterRepo.DeleteCharacterAsync(id);

            if (characterModel == null) return NotFound();

            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCharacter([FromRoute] int id, [FromBody] CharacterUpdateDTO characterDto)
        {
            var characterModel = await _characterRepo.UpdateCharacterAsync(id, characterDto);

            if (characterModel == null) return NotFound();

            return Ok(characterModel.ToReadDTO());
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetCharactersByBookId([FromRoute] int bookId)
        {
            var characters = await _characterRepo.GetCharactersByBookIdAsync(bookId);

            if (characters == null || characters.Count == 0)
                return NotFound($"No characters found for book ID {bookId}");

            return Ok(characters.Select(c => c.ToReadDTO()));
        }
    }
}