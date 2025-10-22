using BookProject.Models;
using static BookProject.DTOs.CharacterDTOs;

namespace BookProject.Mappers
{
    public static class CharacterMappers
    {
        public static Character ToCharacterFromCreateDTO(this CharacterCreateDTO character)
        {
            return new Character
            {
                
                Name = character.Name,
                Description = character.Description,
                Verified = character.Verified,
                BookId = character.BookId
            };
        }

        public static CharacterReadDTO ToReadDTO(this Character character)
        {
            return new CharacterReadDTO
            {
                CharacterId = character.CharacterId,
                Name = character.Name,
                Description = character.Description,
                Verified = character.Verified,
                BookId = character.BookId,
                BookTitle = character.Book?.Title
            };
        }


    }
}
