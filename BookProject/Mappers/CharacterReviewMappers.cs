using BookProject.Models;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Mappers
{
    public static class CharacterReviewMappers
    {
        public static CharacterReviewReadDTO ToReadDTO(this CharacterReview review)
        {
            return new CharacterReviewReadDTO
            {
                CharacterReviewId = review.CharacterReviewId,
                CharacterId = review.CharacterId,
                UserId = review.UserId,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt
            };
        }

        public static CharacterReview ToCharacterReviewFromCreateDTO(this CharacterReviewCreateDTO dto, int userId)
        {
            return new CharacterReview
            {
                CharacterId = dto.CharacterId,
                UserId = userId,
                ReviewText = dto.ReviewText,
                Rating = dto.Rating
            };
        }
    }
}
