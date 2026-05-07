using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class ImageGenerationDTOs
    {
        public class GenerateRequestDto
        {
            [Range(1, int.MaxValue, ErrorMessage = "A valid SceneId is required.")]
            public int SceneId { get; set; }

            [Required]
            [StringLength(1000, MinimumLength = 2, ErrorMessage = "Prompt must be between 2 and 1000 characters.")]
            public string Prompt { get; set; } = null!;

            public List<int>? CharacterIds { get; set; }
        }
    }
}