using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using BookProject.Services;

namespace BookProject.Interfaces
{
    public interface IImageGenerationRepository
    {
        Task<ImageGeneration> GenerateAndSaveAsync(int userId, int sceneId, string prompt, string outputFolder, ImageGenerationService imageService, List<int>? characterIds = null);

        Task<PagedResult<ImageGeneration>> GetAllGenerationsAsync(ImageGenerationQueryObject query);

        Task<List<ImageGeneration>> GetGenerationsByUserAsync(int userId);

        Task<ImageGeneration?> GetGenerationByIdAsync(int generationId);

        Task<List<ImageGeneration>> GetGenerationsBySceneAsync(int sceneId);
    }
}