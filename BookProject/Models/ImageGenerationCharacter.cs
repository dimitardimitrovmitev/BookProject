namespace BookProject.Models
{
    public class ImageGenerationCharacter
    {
        public int ImageGenerationId { get; set; }
        public ImageGeneration ImageGeneration { get; set; } = null!;

        public int CharacterId { get; set; }
        public Character Character { get; set; } = null!;
    }
}
