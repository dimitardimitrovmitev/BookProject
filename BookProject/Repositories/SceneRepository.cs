using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.SceneDTOs;

namespace BookProject.Repositories
{
    public class SceneRepository : ISceneRepository
    {
        private readonly ApplicationDBContext _context;

        public SceneRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Scene>> GetAllScenesAsync(SceneQueryObject query)
        {
            var scenes = _context.Scenes
                .Include(s => s.SceneCharacters)
                    .ThenInclude(sc => sc.Character)
                .Include(s => s.Generations)
                .AsQueryable();

            if (query.UserId.HasValue)
                scenes = scenes.Where(s => s.UserId == query.UserId.Value);

            if (!string.IsNullOrWhiteSpace(query.CharacterName))
            {
                var name = query.CharacterName.Trim().ToLower();
                scenes = scenes.Where(s =>
                    s.SceneCharacters.Any(sc =>
                        sc.Character.Name.ToLower().Contains(name)));
            }

            scenes = query.SortBy switch
            {
                SceneSortBy.Title => query.SortDescending ? scenes.OrderByDescending(s => s.Title) : scenes.OrderBy(s => s.Title),
                _ => query.SortDescending ? scenes.OrderByDescending(s => s.CreatedAt) : scenes.OrderBy(s => s.CreatedAt),
            };

            var totalCount = await scenes.CountAsync();

            var items = await scenes
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Scene>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<Scene?> GetSceneByIdAsync(int id)
        {
            return await _context.Scenes
                .Include(s => s.SceneCharacters)
                .Include(s => s.Generations)
                .FirstOrDefaultAsync(s => s.SceneId == id);
        }

        public async Task<List<Scene>> GetScenesByUserIdAsync(int userId)
        {
            return await _context.Scenes
                .Where(s => s.UserId == userId)
                .Include(s => s.SceneCharacters)
                .Include(s => s.Generations)
                .ToListAsync();
        }

        public async Task<Scene> CreateSceneAsync(Scene scene, List<int>? characterIds)
        {
            await _context.Scenes.AddAsync(scene);
            await _context.SaveChangesAsync();

            if (characterIds != null && characterIds.Any())
            {
                var validCharacterIds = await _context.Characters
                    .Where(c => characterIds.Contains(c.CharacterId))
                    .Select(c => c.CharacterId)
                    .ToListAsync();

                if (validCharacterIds.Count != characterIds.Count)
                    throw new Exception("One or more Character IDs are invalid.");

                foreach (var charId in validCharacterIds)
                {
                    _context.SceneCharacters.Add(new SceneCharacter
                    {
                        SceneId = scene.SceneId,
                        CharacterId = charId
                    });
                }

                await _context.SaveChangesAsync();
            }

            return await GetSceneByIdAsync(scene.SceneId);
        }

        public async Task<Scene?> UpdateSceneAsync(int id, int userId, SceneUpdateDTO sceneDto)
        {
            var existingScene = await _context.Scenes
                .Include(s => s.SceneCharacters)
                .FirstOrDefaultAsync(s => s.SceneId == id && s.UserId == userId);

            if (existingScene == null) return null;

            existingScene.Title = sceneDto.Title;

            if (sceneDto.CharacterIds != null)
            {
                _context.SceneCharacters.RemoveRange(existingScene.SceneCharacters);

                foreach (var charId in sceneDto.CharacterIds)
                {
                    _context.SceneCharacters.Add(new SceneCharacter
                    {
                        SceneId = existingScene.SceneId,
                        CharacterId = charId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return await GetSceneByIdAsync(id);
        }

        public async Task<Scene?> DeleteSceneAsync(int id, int userId)
        {
            var scene = await _context.Scenes
                .FirstOrDefaultAsync(s => s.SceneId == id && s.UserId == userId);

            if (scene == null) return null;

            _context.Scenes.Remove(scene);
            await _context.SaveChangesAsync();
            return scene;
        }
    }
}
