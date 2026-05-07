using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public static class CharacterReviewDTOs
    {
        public class CharacterReviewReadDTO
        {
            public int CharacterReviewId { get; set; }
            public int CharacterId { get; set; }
            public int UserId { get; set; }
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }

        public class CharacterReviewCreateDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "A valid CharacterId is required.")]
            public int CharacterId { get; set; }

            [Required]
            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string ReviewText { get; set; } = null!;

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
            public int Rating { get; set; }
        }

        public class CharacterReviewUpdateDTO
        {
            [Required]
            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string ReviewText { get; set; } = null!;

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
            public int Rating { get; set; }
        }
    }
}