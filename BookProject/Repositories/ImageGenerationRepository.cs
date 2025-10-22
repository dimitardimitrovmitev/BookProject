using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.Services;
using Microsoft.EntityFrameworkCore;

namespace BookProject.Repositories
{


    public class ImageGenerationRepository : IImageGenerationRepository
    {

        private readonly ApplicationDBContext _context;

        public ImageGenerationRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ImageGeneration> GenerateAndSaveAsync(int userId, int sceneId, string prompt, string outputFolder, ImageGenerationService imageService, List<int>? characterIds = null)
        {
            var imageUrl = await imageService.GenerateImageAsync(prompt, outputFolder);

            var imageRecord = new ImageGeneration
            {
                UserId = userId,
                SceneId = sceneId,
                Prompt = prompt,
                ImageUrl = imageUrl,
                GeneratedAt = DateTime.UtcNow
            };

            // 1️⃣ Add and save the ImageGeneration first so it gets an ID
            _context.ImageGenerations.Add(imageRecord);
            await _context.SaveChangesAsync();

            // 2️⃣ Now safely link characters using the generated ID
            if (characterIds != null && characterIds.Any())
            {
                foreach (var charId in characterIds)
                {
                    _context.ImageGenerationCharacters.Add(new ImageGenerationCharacter
                    {
                        ImageGenerationId = imageRecord.ImageGenerationId,
                        CharacterId = charId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return imageRecord;
        }

        public Task<List<ImageGeneration>> GetAllGenerationsAsync()
        {
            return _context.ImageGenerations.ToListAsync();
        }

        public async Task<ImageGeneration?> GetGenerationByIdAsync(int generationId)
        {
            return await _context.ImageGenerations
                .FirstOrDefaultAsync(ig => ig.ImageGenerationId == generationId);
        }

        public async Task<List<ImageGeneration>> GetGenerationsBySceneAsync(int sceneId)
        {
            return await _context.ImageGenerations
                .Where(ig => ig.SceneId == sceneId)
                .ToListAsync();
        }

        public async Task<List<ImageGeneration>> GetGenerationsByUserAsync(int userId)
        {
            return await _context.ImageGenerations
                .Where(ig => ig.UserId == userId)
                .ToListAsync();
        }
    }
}
