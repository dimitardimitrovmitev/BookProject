namespace BookProject.Models
{
    public class UserBook
    {
        public int UserBookId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public User User { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
