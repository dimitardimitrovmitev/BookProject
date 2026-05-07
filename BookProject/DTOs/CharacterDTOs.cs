using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class CharacterDTOs
    {
        public class CharacterCreateDTO
        {
            [Required]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
            public string Name { get; set; } = null!;

            [Required]
            [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
            public string Description { get; set; } = null!;

            public bool Verified { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "A valid BookId is required.")]
            public int BookId { get; set; }
        }

        public class CharacterReadDTO
        {
            public int CharacterId { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public bool Verified { get; set; }
            public string? BookTitle { get; set; }
            public int BookId { get; set; }
        }

        public class CharacterUpdateDTO
        {
            [Required]
            [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
            public string Name { get; set; } = null!;

            [Required]
            [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
            public string Description { get; set; } = null!;

            public bool Verified { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "A valid BookId is required.")]
            public int BookId { get; set; }
        }
    }
}