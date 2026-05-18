using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public static class BookReviewDTOs
    {
        public class BookReviewReadDTO
        {
            public int BookReviewId { get; set; }
            public int BookId { get; set; }
            public int UserId { get; set; }
            public string? ReviewText { get; set; }
            public int Rating { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class BookReviewCreateDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "A valid BookId is required.")]
            public int BookId { get; set; }

            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string? ReviewText { get; set; }

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
            public int Rating { get; set; }
        }

        public class BookReviewUpdateDTO
        {
            [StringLength(1000, MinimumLength = 5, ErrorMessage = "Review text must be between 5 and 1000 characters.")]
            public string? ReviewText { get; set; }

            [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
            public int Rating { get; set; }
        }
    }
}