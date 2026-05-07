using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.CharacterReviewDTOs;

namespace BookProject.Repositories
{
    public class CharacterReviewRepository : ICharacterReviewRepository
    {
        private readonly ApplicationDBContext _context;

        public CharacterReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<CharacterReview>> GetAllReviewsAsync(CharacterReviewQueryObject query)
        {
            var reviews = _context.CharacterReviews.Include(r => r.Character).AsQueryable();

            if (query.CharacterId.HasValue)
                reviews = reviews.Where(r => r.CharacterId == query.CharacterId.Value);

            if (!string.IsNullOrWhiteSpace(query.CharacterName))
            {
                var name = query.CharacterName.Trim().ToLower();
                reviews = reviews.Where(r => r.Character.Name.ToLower().Contains(name));
            }

            if (query.UserId.HasValue)
                reviews = reviews.Where(r => r.UserId == query.UserId.Value);

            reviews = query.SortBy switch
            {
                CharacterReviewSortBy.Rating => query.SortDescending ? reviews.OrderByDescending(r => r.Rating) : reviews.OrderBy(r => r.Rating),
                _ => query.SortDescending ? reviews.OrderByDescending(r => r.CreatedAt) : reviews.OrderBy(r => r.CreatedAt),
            };

            var totalCount = await reviews.CountAsync();

            var items = await reviews
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<CharacterReview>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<CharacterReview?> GetReviewByIdAsync(int id)
        {
            return await _context.CharacterReviews.FindAsync(id);
        }

        public async Task<CharacterReview> CreateReviewAsync(CharacterReview review)
        {
            await _context.CharacterReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<CharacterReview?> UpdateReviewAsync(int id, int userId, CharacterReviewUpdateDTO dto)
        {
            var existing = await _context.CharacterReviews
                .FirstOrDefaultAsync(r => r.CharacterReviewId == id && r.UserId == userId);

            if (existing == null) return null;

            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<CharacterReview?> DeleteReviewAsync(int id, int userId)
        {
            var review = await _context.CharacterReviews
                .FirstOrDefaultAsync(r => r.CharacterReviewId == id && r.UserId == userId);

            if (review == null) return null;

            _context.CharacterReviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}