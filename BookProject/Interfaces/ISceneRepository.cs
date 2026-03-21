using BookProject.Models;
using static BookProject.DTOs.BookDTOs;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Interfaces
{
    public interface ISceneRepository
    {
        Task<List<Scene>> GetAllScenesAsync();
        Task<Scene?> GetSceneByIdAsync(int id);

        Task<Scene> CreateSceneAsync(Scene scene, List<int>? characterIds);
        Task<Scene?> UpdateSceneAsync(int id, SceneUpdateDTO sceneDto);
        Task<Scene?> DeleteSceneAsync(int id);

        Task<List<Scene>> GetScenesByUserIdAsync(int userId);
    }
}
