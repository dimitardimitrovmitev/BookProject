using BookProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class UserBookDTOs
    {
        public class UserBookCreateDTO
        {
            [Range(1, int.MaxValue, ErrorMessage = "A valid BookId is required.")]
            public int BookId { get; set; }
        }

        public class UserBookReadDTO
        {
            public int UserId { get; set; }
            public int BookId { get; set; }
            public ReadingStatus Status { get; set; }
            public DateTime? ReadDate { get; set; }
            public float? UserRating { get; set; }
        }

        public class UserBookUpdateStatusDTO
        {
            [Required]
            public ReadingStatus Status { get; set; }
        }

        public class UserBookRateDTO
        {
            [Range(1.0, 5.0, ErrorMessage = "Rating must be between 1 and 5.")]
            public float UserRating { get; set; }
        }
    }
}