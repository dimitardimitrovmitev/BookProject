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
            public int Rating { get; set; }
        }

        public class CharacterReviewCreateDTO
        {
            public int CharacterId { get; set; }
            public int UserId { get; set; }
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }

        public class CharacterReviewUpdateDTO
        {
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }
    }
}