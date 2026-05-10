namespace BookProject.Models
{
    public class CharacterReview
    {
        public int CharacterReviewId { get; set; }
        public int CharacterId { get; set; }
        public int UserId { get; set; }

        public string ReviewText { get; set; } = null!;

        // Sub-ratings (1.0 – 5.0)
        public float DepthComplexity { get; set; }
        public float CharacterDevelopment { get; set; }
        public float ConsistencyBelievability { get; set; }
        public float ImpactOnStory { get; set; }
        public float Memorability { get; set; }

        // Stored average of the five sub-ratings — updated on every create/update
        public float OverallRating { get; set; }

        // User's personal AI-generated portrait for this character
        public string? AiImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Character Character { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}