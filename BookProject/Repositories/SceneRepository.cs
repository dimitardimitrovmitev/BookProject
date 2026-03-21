using BookProject.Data;
using BookProject.DTOs;
using BookProject.Interfaces;
using BookProject.Models;
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

        public async Task<Scene> CreateSceneAsync(Scene scene, List<int>? characterIds)
        {
            await _context.Scenes.AddAsync(scene);
            await _context.SaveChangesAsync();

            if (characterIds != null && characterIds.Any())
            {
                // ✅ validate + filter valid IDs
                var validCharacterIds = await _context.Characters
                    .Where(c => characterIds.Contains(c.CharacterId))
                    .Select(c => c.CharacterId)
                    .ToListAsync();

                // ❗ optional strict check
                if (validCharacterIds.Count != characterIds.Count)
                {
                    throw new Exception("One or more Character IDs are invalid.");
                }

                // ✅ add relations
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

        public async Task<Scene?> DeleteSceneAsync(int id)
        {
            var scene = await _context.Scenes.FindAsync(id);
            if (scene == null)
                return null;

            _context.Scenes.Remove(scene);
            await _context.SaveChangesAsync();

            return scene;
        }

        public async Task<List<Scene>> GetAllScenesAsync()
        {
            return await _context.Scenes
                .Include(s => s.SceneCharacters)
                .Include(s => s.Generations)
                .ToListAsync();
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

        public async Task<Scene?> UpdateSceneAsync(int id, SceneUpdateDTO sceneDto)
        {
            var existingScene = await _context.Scenes
                .Include(s => s.SceneCharacters)
                .FirstOrDefaultAsync(s => s.SceneId == id);

            if (existingScene == null)
                return null;

            // update basic fields
            existingScene.Title = sceneDto.Title;

            // update characters (reset and re-add)
            if (sceneDto.CharacterIds != null)
            {
                // remove old
                _context.SceneCharacters.RemoveRange(existingScene.SceneCharacters);

                // add new
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
    }
}

