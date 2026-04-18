namespace BookProject.DTOs
{
    public static class BookReviewDTOs
    {
        public class BookReviewReadDTO
        {
            public int BookReviewId { get; set; }
            public int BookId { get; set; }
            public int UserId { get; set; }
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }

        public class BookReviewCreateDTO
        {
            public int BookId { get; set; }
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }

        public class BookReviewUpdateDTO
        {
            public string ReviewText { get; set; } = null!;
            public int Rating { get; set; }
        }
    }
}