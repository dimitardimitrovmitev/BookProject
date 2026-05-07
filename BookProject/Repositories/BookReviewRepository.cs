using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
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

        public async Task<PagedResult<BookReview>> GetAllReviewsAsync(BookReviewQueryObject query)
        {
            var reviews = _context.BookReviews.Include(r => r.Book).AsQueryable();

            if (query.BookId.HasValue)
                reviews = reviews.Where(r => r.BookId == query.BookId.Value);

            if (!string.IsNullOrWhiteSpace(query.BookTitle))
            {
                var title = query.BookTitle.Trim().ToLower();
                reviews = reviews.Where(r => r.Book.Title.ToLower().Contains(title));
            }

            if (query.UserId.HasValue)
                reviews = reviews.Where(r => r.UserId == query.UserId.Value);

            reviews = query.SortBy switch
            {
                BookReviewSortBy.Rating => query.SortDescending ? reviews.OrderByDescending(r => r.Rating) : reviews.OrderBy(r => r.Rating),
                _ => query.SortDescending ? reviews.OrderByDescending(r => r.CreatedAt) : reviews.OrderBy(r => r.CreatedAt),
            };

            var totalCount = await reviews.CountAsync();

            var items = await reviews
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<BookReview>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
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

        public async Task<BookReview?> UpdateReviewAsync(int id, int userId, BookReviewUpdateDTO dto)
        {
            var existing = await _context.BookReviews
                .FirstOrDefaultAsync(r => r.BookReviewId == id && r.UserId == userId);

            if (existing == null) return null;

            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<BookReview?> DeleteReviewAsync(int id, int userId)
        {
            var review = await _context.BookReviews
                .FirstOrDefaultAsync(r => r.BookReviewId == id && r.UserId == userId);

            if (review == null) return null;

            _context.BookReviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}