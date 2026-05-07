using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Interfaces
{
    public interface ISceneRepository
    {
        Task<PagedResult<Scene>> GetAllScenesAsync(SceneQueryObject query);
        Task<Scene?> GetSceneByIdAsync(int id);
        Task<Scene> CreateSceneAsync(Scene scene, List<int>? characterIds);
        Task<Scene?> UpdateSceneAsync(int id, int userId, SceneUpdateDTO sceneDto);
        Task<Scene?> DeleteSceneAsync(int id, int userId);

        Task<List<Scene>> GetScenesByUserIdAsync(int userId);
    }
}
