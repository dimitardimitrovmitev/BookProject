using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.Repositories;
using BookProject.Services;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.ImageGenerationDTOs;

namespace BookProject.Controllers
{
    [ApiController]
    [Route("imagegeneration")]
    public class ImageGenerationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ImageGenerationService _imageService;
        private readonly IWebHostEnvironment _env;
        private readonly IImageGenerationRepository _imageRepo;
        public ImageGenerationController(ApplicationDBContext context, ImageGenerationService imageService, IWebHostEnvironment env, IImageGenerationRepository imageRepo)
        {
            _context = context;
            _imageService = imageService;
            _env = env;
            _imageRepo = imageRepo;
        }

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
            if (string.IsNullOrWhiteSpace(request.Prompt))
                return BadRequest("Prompt cannot be empty.");

            var outputFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "generated");

            // Include characterIds now
            var imageRecord = await _imageRepo.GenerateAndSaveAsync(
                request.UserId,
                request.SceneId,
                request.Prompt,
                outputFolder,
                _imageService,
                request.CharacterIds   // ← pass them through
            );

            return Ok(new
            {
                id = imageRecord.ImageGenerationId,
                url = imageRecord.ImageUrl,
                generatedAt = imageRecord.GeneratedAt,
                characters = request.CharacterIds    // optional, just for response context
            });
        }

    }
}