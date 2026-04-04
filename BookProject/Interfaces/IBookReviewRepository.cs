using BookProject.Models;
using static BookProject.DTOs.BookReviewDTOs;

namespace BookProject.Interfaces
{
    public interface IBookReviewRepository
    {
        Task<List<BookReview>> GetAllReviewsAsync();
        Task<BookReview?> GetReviewByIdAsync(int id);
        Task<List<BookReview>> GetReviewsByBookIdAsync(int bookId);
        Task<BookReview> CreateReviewAsync(BookReview review);
        Task<BookReview?> UpdateReviewAsync(int id, BookReviewUpdateDTO dto);
        Task<BookReview?> DeleteReviewAsync(int id);
    }
}