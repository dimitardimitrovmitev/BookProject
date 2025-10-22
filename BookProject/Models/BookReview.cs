namespace BookProject.Models
{
    public class BookReview
    {
        public int BookReviewId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string ReviewText { get; set; } = null!;
        public int Rating { get; set; }

        public Book Book { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
