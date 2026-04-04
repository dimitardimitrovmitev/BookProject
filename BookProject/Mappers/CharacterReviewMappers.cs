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
                Rating = review.Rating
            };
        }

        public static CharacterReview ToCharacterReviewFromCreateDTO(this CharacterReviewCreateDTO dto)
        {
            return new CharacterReview
            {
                CharacterId = dto.CharacterId,
                UserId = dto.UserId,
                ReviewText = dto.ReviewText,
                Rating = dto.Rating
            };
        }
    }
}