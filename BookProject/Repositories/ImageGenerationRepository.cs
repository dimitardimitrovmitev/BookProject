using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
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

            _context.ImageGenerations.Add(imageRecord);
            await _context.SaveChangesAsync();

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

        public async Task<PagedResult<ImageGeneration>> GetAllGenerationsAsync(ImageGenerationQueryObject query)
        {
            var generations = _context.ImageGenerations
                .Include(ig => ig.ImageGenerationCharacters)
                    .ThenInclude(igc => igc.Character)
                        .ThenInclude(c => c.Book)
                .Include(ig => ig.Scene)
                    .ThenInclude(s => s.SceneCharacters)
                        .ThenInclude(sc => sc.Character)
                .AsQueryable();

            if (query.UserId.HasValue)
                generations = generations.Where(ig => ig.UserId == query.UserId.Value);

            if (query.SceneId.HasValue)
                generations = generations.Where(ig => ig.SceneId == query.SceneId.Value);

            if (query.CharacterId.HasValue)
                generations = generations.Where(ig =>
                    ig.ImageGenerationCharacters.Any(igc => igc.CharacterId == query.CharacterId.Value));

            if (!string.IsNullOrWhiteSpace(query.CharacterName))
            {
                var name = query.CharacterName.Trim().ToLower();
                generations = generations.Where(ig =>
                    ig.ImageGenerationCharacters.Any(igc =>
                        igc.Character.Name.ToLower().Contains(name)));
            }

            if (query.BookId.HasValue)
                generations = generations.Where(ig =>
                    ig.ImageGenerationCharacters.Any(igc =>
                        igc.Character.BookId == query.BookId.Value));

            if (!string.IsNullOrWhiteSpace(query.BookTitle))
            {
                var title = query.BookTitle.Trim().ToLower();
                generations = generations.Where(ig =>
                    ig.ImageGenerationCharacters.Any(igc =>
                        igc.Character.Book.Title.ToLower().Contains(title)));
            }

            // GeneratedAt is the only sort option — direction is controllable
            generations = query.SortDescending
                ? generations.OrderByDescending(ig => ig.GeneratedAt)
                : generations.OrderBy(ig => ig.GeneratedAt);

            var totalCount = await generations.CountAsync();

            var items = await generations
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<ImageGeneration>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
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