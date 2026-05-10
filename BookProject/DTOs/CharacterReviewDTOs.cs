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

            public float DepthComplexity { get; set; }
            public float CharacterDevelopment { get; set; }
            public float ConsistencyBelievability { get; set; }
            public float ImpactOnStory { get; set; }
            public float Memorability { get; set; }
            public float OverallRating { get; set; }

            public string? AiImageUrl { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class CharacterReviewCreateDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "A valid CharacterId is required.")]
            public int CharacterId { get; set; }

            [Required]
            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string ReviewText { get; set; } = null!;

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float DepthComplexity { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float CharacterDevelopment { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float ConsistencyBelievability { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float ImpactOnStory { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float Memorability { get; set; }

            public string? AiImageUrl { get; set; }
        }

        public class CharacterReviewUpdateDTO
        {
            [Required]
            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string ReviewText { get; set; } = null!;

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float DepthComplexity { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float CharacterDevelopment { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float ConsistencyBelievability { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float ImpactOnStory { get; set; }

            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1.0 and 5.0.")]
            public float Memorability { get; set; }

            public string? AiImageUrl { get; set; }
        }
    }
}