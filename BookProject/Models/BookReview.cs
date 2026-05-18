namespace BookProject.Models
{
    public class BookReview
    {
        public int BookReviewId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}