using BookProject.Interfaces;
using BookProject.Mappers;
using BookProject.QueryObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [Route("scene")]
    [ApiController]
    public class SceneController : ControllerBase
    {
        private readonly ISceneRepository _sceneRepo;

        public SceneController(ISceneRepository sceneRepo)
        {
            _sceneRepo = sceneRepo;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        public async Task<IActionResult> GetAllScenes([FromQuery] SceneQueryObject query)
        {
            var result = await _sceneRepo.GetAllScenesAsync(query);

            return Ok(new
            {
                items = result.Items.Select(s => s.ToReadDTO()),
                result.TotalCount,
                result.PageNumber,
                result.PageSize,
                result.TotalPages
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSceneById([FromRoute] int id)
        {
            var scene = await _sceneRepo.GetSceneByIdAsync(id);
            if (scene == null) return NotFound();
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
            var scene = dto.ToSceneFromCreateDTO(GetCurrentUserId());
            var createdScene = await _sceneRepo.CreateSceneAsync(scene, dto.CharacterIds);
            return CreatedAtAction(nameof(GetSceneById),
                new { id = createdScene.SceneId },
                createdScene.ToReadDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScene([FromRoute] int id, [FromBody] SceneUpdateDTO dto)
        {
            var updatedScene = await _sceneRepo.UpdateSceneAsync(id, GetCurrentUserId(), dto);
            if (updatedScene == null) return NotFound();
            return Ok(updatedScene.ToReadDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScene([FromRoute] int id)
        {
            var deletedScene = await _sceneRepo.DeleteSceneAsync(id, GetCurrentUserId());
            if (deletedScene == null) return NotFound();
            return NoContent();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyScenes()
        {
            var scenes = await _sceneRepo.GetScenesByUserIdAsync(GetCurrentUserId());
            if (scenes == null || scenes.Count == 0)
                return NotFound("No scenes found.");
            return Ok(scenes.Select(s => s.ToReadDTO()));
        }
    }
}
