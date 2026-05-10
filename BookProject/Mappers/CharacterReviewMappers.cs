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
                DepthComplexity = review.DepthComplexity,
                CharacterDevelopment = review.CharacterDevelopment,
                ConsistencyBelievability = review.ConsistencyBelievability,
                ImpactOnStory = review.ImpactOnStory,
                Memorability = review.Memorability,
                OverallRating = review.OverallRating,
                AiImageUrl = review.AiImageUrl,
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
                DepthComplexity = dto.DepthComplexity,
                CharacterDevelopment = dto.CharacterDevelopment,
                ConsistencyBelievability = dto.ConsistencyBelievability,
                ImpactOnStory = dto.ImpactOnStory,
                Memorability = dto.Memorability,
                OverallRating = (dto.DepthComplexity + dto.CharacterDevelopment + dto.ConsistencyBelievability + dto.ImpactOnStory + dto.Memorability) / 5f,
                AiImageUrl = dto.AiImageUrl
            };
        }
    }
}