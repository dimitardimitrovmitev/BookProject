namespace BookProject.Models
{
    public class ImageGeneration
    {
        public int ImageGenerationId { get; set; }
        public int UserId { get; set; }
        public int SceneId { get; set; }

        public string ImageUrl { get; set; } = null!; // where you store the generated image
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Scene Scene { get; set; } = null!;

        public string Prompt { get; set; } = null!;


        public ICollection<ImageGenerationCharacter> ImageGenerationCharacters { get; set; } = new List<ImageGenerationCharacter>();

    }
}
