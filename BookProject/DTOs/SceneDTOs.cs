using BookProject.Models;

namespace BookProject.DTOs
{
    public class SceneDTOs
    {

        public class SceneCreateDTO
        {
            public string Title { get; set; } = null!;

            
            public List<int>? CharacterIds { get; set; }
        }

        public class SceneReadDTO
        {
            public int SceneId { get; set; }
            public string Title { get; set; } = null!;
            public DateTime CreatedAt { get; set; }

            public int UserId { get; set; }

            
            public List<int>? CharacterIds { get; set; }

                
            public List<string>? ImageUrls { get; set; }
        }

        public class SceneUpdateDTO
        {
            public string Title { get; set; } = null!;

            
            public List<int>? CharacterIds { get; set; }
        }
    }
}
