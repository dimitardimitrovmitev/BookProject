namespace BookProject.Models
{
    public class SceneCharacter
    {
        public int SceneCharacterId { get; set; }
        public int SceneId { get; set; }
        public int CharacterId { get; set; }

        public Scene Scene { get; set; } = null!;
        public Character Character { get; set; } = null!;
    }
}