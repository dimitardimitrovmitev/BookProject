namespace BookProject.DTOs
{
    public class UserBookDTOs
    {
        public class UserBookCreateDTO
        {
            public int UserId { get; set; }
            public int BookId { get; set; }
        }

        public class UserBookReadDTO
        {
            public int UserBookId { get; set; }
            public int UserId { get; set; }
            public int BookId { get; set; }
            public bool IsRead { get; set; }
            public DateTime? ReadDate { get; set; }
            public float? UserRating { get; set; }
        }

        public class UserBookMarkReadDTO
        {
            public DateTime? ReadDate { get; set; }
        }

        public class UserBookRateDTO
        {
            public float UserRating { get; set; }
        }
    }
}