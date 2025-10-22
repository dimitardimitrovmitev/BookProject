using BookProject.Models;
using static BookProject.DTOs.CharacterDTOs;

namespace BookProject.Interfaces
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetAllCharactersAsync();
        Task<Character?> GetCharacterByIdAsync(int id);
        Task<Character> AddCharacterAsync(Character character);
        Task<Character?> UpdateCharacterAsync(int id, CharacterUpdateDTO character);
        Task<Character?> DeleteCharacterAsync(int id);

        Task<List<Character>> GetCharactersByBookIdAsync(int id);
    }
}
