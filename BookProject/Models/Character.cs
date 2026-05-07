namespace BookProject.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        public int BookId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!; 
        public bool Verified { get; set; } = false;

        public Book Book { get; set; } = null!;
        public ICollection<CharacterReview> Reviews { get; set; } = new List<CharacterReview>();

        public ICollection<ImageGenerationCharacter> ImageGenerationCharacters { get; set; } = new List<ImageGenerationCharacter>();

        public ICollection<SceneCharacter> SceneCharacters { get; set; } = new List<SceneCharacter>();

    }
}