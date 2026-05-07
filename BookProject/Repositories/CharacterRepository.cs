using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
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

        public async Task<PagedResult<Character>> GetAllCharactersAsync(CharacterQueryObject query)
        {
            var characters = _context.Characters.Include(c => c.Book).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                characters = characters.Where(c => c.Name.ToLower().Contains(search));
            }

            if (query.BookId.HasValue)
                characters = characters.Where(c => c.BookId == query.BookId.Value);

            if (!string.IsNullOrWhiteSpace(query.BookTitle))
            {
                var title = query.BookTitle.Trim().ToLower();
                characters = characters.Where(c => c.Book.Title.ToLower().Contains(title));
            }

            if (query.Verified.HasValue)
                characters = characters.Where(c => c.Verified == query.Verified.Value);

            characters = query.SortBy switch
            {
                CharacterSortBy.Verified => query.SortDescending ? characters.OrderByDescending(c => c.Verified) : characters.OrderBy(c => c.Verified),
                CharacterSortBy.BookTitle => query.SortDescending ? characters.OrderByDescending(c => c.Book.Title) : characters.OrderBy(c => c.Book.Title),
                _ => query.SortDescending ? characters.OrderByDescending(c => c.Name) : characters.OrderBy(c => c.Name),
            };

            var totalCount = await characters.CountAsync();

            var items = await characters
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Character>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Character>> GetCharactersByBookIdAsync(int bookId, CharacterQueryObject query)
        {
            var characters = _context.Characters
                .Include(c => c.Book)
                .Where(c => c.BookId == bookId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                characters = characters.Where(c => c.Name.ToLower().Contains(search));
            }

            if (query.Verified.HasValue)
                characters = characters.Where(c => c.Verified == query.Verified.Value);

            characters = query.SortBy switch
            {
                CharacterSortBy.Verified => query.SortDescending ? characters.OrderByDescending(c => c.Verified) : characters.OrderBy(c => c.Verified),
                _ => query.SortDescending ? characters.OrderByDescending(c => c.Name) : characters.OrderBy(c => c.Name),
            };

            var totalCount = await characters.CountAsync();

            var items = await characters
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Character>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public async Task<Character> AddCharacterAsync(Character character)
        {
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            return character;
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

        public async Task<Character?> DeleteCharacterAsync(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(x => x.CharacterId == id);

            if (character == null) return null;

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return character;
        }
    }
}