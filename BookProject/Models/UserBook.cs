namespace BookProject.Models
{
    public enum ReadingStatus
    {
        WantToRead,
        Reading,
        Read,
        DNF
    }

    public class UserBook
    {
        public int UserId { get; set; }
        public int BookId { get; set; }

        public ReadingStatus Status { get; set; } = ReadingStatus.WantToRead;
        public DateTime? ReadDate { get; set; }
        public float? UserRating { get; set; }

        public User User { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}