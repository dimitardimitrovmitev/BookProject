namespace BookProject.DTOs
{
    public class CharacterDTOs
    {
        public class CharacterCreateDTO
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public bool Verified { get; set; }

            public int BookId { get; set; }
        }

        public class CharacterReadDTO
        {
            public int CharacterId { get; set; }
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public bool Verified { get; set; }

            public string? BookTitle { get; set; }
            public int BookId { get; set; }
    
        }

        public class CharacterUpdateDTO
        {
            public string Name { get; set; } = null!;
            public string Description { get; set; } = null!;
            public bool Verified { get; set; }

            public int BookId { get; set; }
        }
    }
}
