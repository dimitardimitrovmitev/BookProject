using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Repositories
{
    public class UserBookRepository : IUserBookRepository
    {
        private readonly ApplicationDBContext _context;

        public UserBookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<UserBook>> GetAllUserBooksAsync()
        {
            return await _context.UserBooks.ToListAsync();
        }

        public async Task<List<UserBook>> GetBooksByUserIdAsync(int userId)
        {
            return await _context.UserBooks
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserBook?> GetUserBookAsync(int userId, int bookId)
        {
            return await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);
        }

        public async Task<UserBook> AddUserBookAsync(UserBook userBook)
        {
            await _context.UserBooks.AddAsync(userBook);
            await _context.SaveChangesAsync();
            return userBook;
        }

        public async Task<UserBook?> MarkAsReadAsync(int userId, int bookId, UserBookMarkReadDTO dto)
        {
            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

            if (userBook == null) return null;

            userBook.IsRead = true;
            userBook.ReadDate = dto.ReadDate ?? DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return userBook;
        }

        public async Task<UserBook?> RateBookAsync(int userId, int bookId, UserBookRateDTO dto)
        {
            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

            if (userBook == null) return null;

            userBook.UserRating = dto.UserRating;

            await _context.SaveChangesAsync();
            return userBook;
        }

        public async Task<UserBook?> RemoveUserBookAsync(int userId, int bookId)
        {
            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

            if (userBook == null) return null;

            _context.UserBooks.Remove(userBook);
            await _context.SaveChangesAsync();
            return userBook;
        }
    }
}