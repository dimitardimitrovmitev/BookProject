using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Interfaces
{
    public interface ICharacterReviewRepository
    {
        Task<PagedResult<CharacterReview>> GetAllReviewsAsync(CharacterReviewQueryObject query);
        Task<CharacterReview?> GetReviewByIdAsync(int id);
        Task<CharacterReview> CreateReviewAsync(CharacterReview review);
        Task<CharacterReview?> UpdateReviewAsync(int id, CharacterReviewUpdateDTO dto);
        Task<CharacterReview?> DeleteReviewAsync(int id);
    }
}