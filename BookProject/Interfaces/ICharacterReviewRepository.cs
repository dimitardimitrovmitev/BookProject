using BookProject.Models;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Interfaces
{
    public interface ICharacterReviewRepository
    {
        Task<List<CharacterReview>> GetAllReviewsAsync();
        Task<CharacterReview?> GetReviewByIdAsync(int id);
        Task<CharacterReview> CreateReviewAsync(CharacterReview review);
        Task<CharacterReview?> UpdateReviewAsync(int id, CharacterReviewUpdateDTO dto);
        Task<CharacterReview?> DeleteReviewAsync(int id);
    }
}
