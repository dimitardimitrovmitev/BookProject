using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
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

        public async Task<List<CharacterReview>> GetAllReviewsAsync()
        {
            return await _context.CharacterReviews.ToListAsync();
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

        public async Task<CharacterReview?> UpdateReviewAsync(int id, CharacterReviewUpdateDTO dto)
        {
            var existing = await _context.CharacterReviews.FirstOrDefaultAsync(r => r.CharacterReviewId == id);

            if (existing == null) return null;

            existing.ReviewText = dto.ReviewText;
            existing.Rating = dto.Rating;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<CharacterReview?> DeleteReviewAsync(int id)
        {
            var review = await _context.CharacterReviews.FindAsync(id);

            if (review == null) return null;

            _context.CharacterReviews.Remove(review);
            await _context.SaveChangesAsync();
            return review;
        }
    }
}