namespace BookProject.Models
{
    public class Scene
    {
        public int SceneId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public ICollection<SceneCharacter> SceneCharacters { get; set; } = new List<SceneCharacter>();
        public ICollection<ImageGeneration> Generations { get; set; } = new List<ImageGeneration>();
    }
}
