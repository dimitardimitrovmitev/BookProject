using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.BookReviewDTOs;

namespace BookProject.Interfaces
{
    public interface IBookReviewRepository
    {
        Task<PagedResult<BookReview>> GetAllReviewsAsync(BookReviewQueryObject query);
        Task<BookReview?> GetReviewByIdAsync(int id);
        Task<List<BookReview>> GetReviewsByBookIdAsync(int bookId);
        Task<BookReview> CreateReviewAsync(BookReview review);
        Task<BookReview?> UpdateReviewAsync(int id, BookReviewUpdateDTO dto);
        Task<BookReview?> DeleteReviewAsync(int id);
    }
}