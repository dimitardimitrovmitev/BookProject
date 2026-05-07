using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Mappers;
using BookProject.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllCharactersAsync([FromQuery] CharacterQueryObject query)
        {
            var result = await _characterRepo.GetAllCharactersAsync(query);

            return Ok(new
            {
                items = result.Items.Select(c => c.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacterByIdAsync([FromRoute] int id)
        {
            var character = await _characterRepo.GetCharacterByIdAsync(id);
            if (character == null) return NotFound();
            return Ok(character.ToReadDTO());
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetCharactersByBookId([FromRoute] int bookId, [FromQuery] CharacterQueryObject query)
        {
            var result = await _characterRepo.GetCharactersByBookIdAsync(bookId, query);

            if (result.TotalCount == 0)
                return NotFound($"No characters found for book ID {bookId}.");

            return Ok(new
            {
                items = result.Items.Select(c => c.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpPost("manual")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterCreateDTO characterDto)
        {
            var characterModel = characterDto.ToCharacterFromCreateDTO();
            await _characterRepo.AddCharacterAsync(characterModel);
            return CreatedAtAction("GetCharacterById", new { id = characterModel.CharacterId }, characterModel.ToReadDTO());
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCharacter([FromRoute] int id, [FromBody] CharacterUpdateDTO characterDto)
        {
            var characterModel = await _characterRepo.UpdateCharacterAsync(id, characterDto);
            if (characterModel == null) return NotFound();
            return Ok(characterModel.ToReadDTO());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCharacter([FromRoute] int id)
        {
            var characterModel = await _characterRepo.DeleteCharacterAsync(id);
            if (characterModel == null) return NotFound();
            return NoContent();
        }
    }
}
