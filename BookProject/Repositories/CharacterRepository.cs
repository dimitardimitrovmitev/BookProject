using BookProject.Data;
using BookProject.DTOs;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.CharacterDTOs;

namespace BookProject.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDBContext _context;

        public CharacterRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Character> AddCharacterAsync(Character character)
        {
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<Character?> DeleteCharacterAsync(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == id);

            if (character == null) return null;

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            return await _context.Characters.ToListAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public async Task<Character?> UpdateCharacterAsync(int id, CharacterUpdateDTO characterDto)
        {
            var existingCharacter = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == id);

            if (existingCharacter == null) return null;

            existingCharacter.Name = characterDto.Name;
            existingCharacter.Description = characterDto.Description;
            existingCharacter.Verified = characterDto.Verified;
            existingCharacter.BookId = characterDto.BookId;

            await _context.SaveChangesAsync();

            return existingCharacter;
        }

        public async Task<List<Character>> GetCharactersByBookIdAsync(int bookId)
        {
            return await _context.Characters
                .Where(c => c.BookId == bookId)
                .Include(c => c.Book)       // optional — only if you want book info in the DTO
                .ToListAsync();
        }
    }
}
