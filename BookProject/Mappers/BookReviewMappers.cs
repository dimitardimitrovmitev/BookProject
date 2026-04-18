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
                Rating = review.Rating
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