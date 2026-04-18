namespace BookProject.DTOs
{
    public class ImageGenerationDTOs
    {
        public class GenerateRequestDto
        {
            public int SceneId { get; set; }
            public string Prompt { get; set; } = null!;
            public List<int>? CharacterIds { get; set; }
        }
    }
}