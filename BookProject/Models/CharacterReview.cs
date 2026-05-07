namespace BookProject.Models
{
    public class CharacterReview
    {
        public int CharacterReviewId { get; set; }
        public int CharacterId { get; set; }
        public int UserId { get; set; }
        public string ReviewText { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Character Character { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
