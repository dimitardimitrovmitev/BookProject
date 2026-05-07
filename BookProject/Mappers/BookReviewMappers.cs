using BookProject.Models;
using static BookProject.DTOs.BookReviewDTOs;

namespace BookProject.Mappers
{
    public static class BookReviewMappers
    {
        public static BookReviewReadDTO ToReadDTO(this BookReview review)
        {
            return new BookReviewReadDTO
            {
                BookReviewId = review.BookReviewId,
                BookId = review.BookId,
                UserId = review.UserId,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            };
        }

        public static BookReview ToBookReviewFromCreateDTO(this BookReviewCreateDTO dto, int userId)
        {
            return new BookReview
            {
                BookId = dto.BookId,
                UserId = userId,
                ReviewText = dto.ReviewText,
                Rating = dto.Rating
            };
        }
    }
}
