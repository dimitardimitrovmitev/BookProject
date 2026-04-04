using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.BookReviewDTOs;

namespace BookProject.Repositories
{
    public class BookReviewRepository : IBookReviewRepository
    {
        private readonly ApplicationDBContext _context;

        public BookReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<BookReview>> GetAllReviewsAsync()
        {
            return await _context.BookReviews.ToListAsync();
        }

        public async Task<BookReview?> GetReviewByIdAsync(int id)
        {
            return await _context.BookReviews.FindAsync(id);
        }

        public async Task<List<BookReview>> GetReviewsByBookIdAsync(int bookId)
        {
            return await _context.BookReviews
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task<BookReview> CreateReviewAsync(BookReview review)
        {
            await _context.BookReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<BookReview?> UpdateReviewAsync(int id, BookReviewUpdateDTO dto)
        {
            var existing = await _context.BookReviews.FirstOrDefaultAsync(r => r.BookReviewId == id);

            if (existing == null) return null;

            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<BookReview?> DeleteReviewAsync(int id)
        {
            var review = await _context.BookReviews.FindAsync(id);

            if (review == null) return null;

            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}