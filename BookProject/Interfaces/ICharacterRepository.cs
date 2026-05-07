using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.CharacterDTOs;

namespace BookProject.Interfaces
{
    public interface ICharacterRepository
    {
        Task<PagedResult<Character>> GetAllCharactersAsync(CharacterQueryObject query);
        Task<Character?> GetCharacterByIdAsync(int id);
        Task<Character> AddCharacterAsync(Character character);
        Task<Character?> UpdateCharacterAsync(int id, CharacterUpdateDTO character);
        Task<Character?> DeleteCharacterAsync(int id);

        Task<PagedResult<Character>> GetCharactersByBookIdAsync(int bookId, CharacterQueryObject query);
    }
}