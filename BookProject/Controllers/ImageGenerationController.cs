using BookProject.Interfaces;
using BookProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BookProject.DTOs.ImageGenerationDTOs;

namespace BookProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("imagegeneration")]
    public class ImageGenerationController : ControllerBase
    {
        private readonly ImageGenerationService _imageService;
        private readonly IWebHostEnvironment _env;
        private readonly IImageGenerationRepository _imageRepo;

        public ImageGenerationController(ImageGenerationService imageService, IWebHostEnvironment env, IImageGenerationRepository imageRepo)
        {
            _imageService = imageService;
            _env = env;
            _imageRepo = imageRepo;
        }

        private int GetCurrentUserId() =>
            int.Parse(User.FindFirstValue("userId")!);

        [HttpGet]
        public async Task<IActionResult> GetGenerations()
        {
            var generations = await _imageRepo.GetAllGenerationsAsync();
            if (generations == null || generations.Count == 0) return NotFound("No image generations found.");
            return Ok(generations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGeneration([FromRoute] int id)
        {
            var generation = await _imageRepo.GetGenerationByIdAsync(id);
            if (generation is null) return NotFound();
            return Ok(generation);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGenerationsByUser([FromRoute] int userId)
        {
            var generations = await _imageRepo.GetGenerationsByUserAsync(userId);
            if (generations == null || generations.Count == 0) return NotFound("No image generations found for the specified user.");
            return Ok(generations);
        }

        [HttpGet("scene/{sceneId}")]
        public async Task<IActionResult> GetGenerationsByScene([FromRoute] int sceneId)
        {
            var generations = await _imageRepo.GetGenerationsBySceneAsync(sceneId);
            if (generations == null || generations.Count == 0) return NotFound("No image generations found for the specified scene.");
            return Ok(generations);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] GenerateRequestDto request)
        {
            

            var outputFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "generated");

            var imageRecord = await _imageRepo.GenerateAndSaveAsync(
                GetCurrentUserId(),
                request.SceneId,
                request.Prompt,
                outputFolder,
                _imageService,
                request.CharacterIds
            );

            return Ok(new
            {
                id = imageRecord.ImageGenerationId,
                url = imageRecord.ImageUrl,
                generatedAt = imageRecord.GeneratedAt,
                characters = request.CharacterIds
            });
        }
    }
}