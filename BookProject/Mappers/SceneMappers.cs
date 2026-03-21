using BookProject.Models;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Mappers
{
    public static class SceneMappers
    {
        public static Scene ToSceneFromCreateDTO(this SceneCreateDTO dto, int userId)
        {
            return new Scene
            {
                Title = dto.Title,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
        }

        public static SceneReadDTO ToReadDTO(this Scene scene)
        {
            return new SceneReadDTO
            {
                SceneId = scene.SceneId,
                Title = scene.Title,
                CreatedAt = scene.CreatedAt,
                UserId = scene.UserId,

                // extract character IDs from join table
                CharacterIds = scene.SceneCharacters?
                    .Select(sc => sc.CharacterId)
                    .ToList(),

                // extract image URLs
                ImageUrls = scene.Generations?
                    .Select(g => g.ImageUrl)
                    .ToList()
            };
        }
    }
}
