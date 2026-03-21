using BookProject.Interfaces;
using BookProject.Mappers;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Controllers
{
    [Route("scene")]
    [ApiController]
    public class SceneController : ControllerBase
    {
        private readonly ISceneRepository _sceneRepo;

        public SceneController(ISceneRepository sceneRepo)
        {
            _sceneRepo = sceneRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScenes()
        {
            var scenes = await _sceneRepo.GetAllScenesAsync();
            return Ok(scenes.Select(s => s.ToReadDTO()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSceneById([FromRoute] int id)
        {
            var scene = await _sceneRepo.GetSceneByIdAsync(id);

            if (scene == null)
                return NotFound();

            return Ok(scene.ToReadDTO());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetScenesByUserId([FromRoute] int userId)
        {
            var scenes = await _sceneRepo.GetScenesByUserIdAsync(userId);

            if (scenes == null || scenes.Count == 0)
                return NotFound("No scenes found for this user.");

            return Ok(scenes.Select(s => s.ToReadDTO()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateScene([FromBody] SceneCreateDTO dto)
        {
            // TODO: replace with user from JWT later
            int userId = 1;

            var scene = dto.ToSceneFromCreateDTO(userId);

            var createdScene = await _sceneRepo.CreateSceneAsync(scene, dto.CharacterIds);

            return CreatedAtAction(nameof(GetSceneById),
                new { id = createdScene.SceneId },
                createdScene.ToReadDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScene([FromRoute] int id, [FromBody] SceneUpdateDTO dto)
        {
            var updatedScene = await _sceneRepo.UpdateSceneAsync(id, dto);

            if (updatedScene == null)
                return NotFound();

            return Ok(updatedScene.ToReadDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScene([FromRoute] int id)
        {
            var deletedScene = await _sceneRepo.DeleteSceneAsync(id);

            if (deletedScene == null)
                return NotFound();

            return NoContent();
        }
    }
}