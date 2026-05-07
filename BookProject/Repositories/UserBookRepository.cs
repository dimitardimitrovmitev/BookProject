using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
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

        public async Task<PagedResult<UserBook>> GetAllUserBooksAsync(UserBookQueryObject query)
        {
            var userBooks = _context.UserBooks.Include(ub => ub.Book).AsQueryable();

            userBooks = ApplyFilters(userBooks, query);
            userBooks = ApplySort(userBooks, query);

            var totalCount = await userBooks.CountAsync();

            var items = await userBooks
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<UserBook>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<UserBook>> GetBooksByUserIdAsync(int userId, UserBookQueryObject query)
        {
            var userBooks = _context.UserBooks
                .Include(ub => ub.Book)
                .Where(ub => ub.UserId == userId)
                .AsQueryable();

            userBooks = ApplyFilters(userBooks, query);
            userBooks = ApplySort(userBooks, query);

            var totalCount = await userBooks.CountAsync();

            var items = await userBooks
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<UserBook>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        private static IQueryable<UserBook> ApplyFilters(IQueryable<UserBook> query, UserBookQueryObject filter)
        {
            if (filter.Status.HasValue)
                query = query.Where(ub => ub.Status == filter.Status.Value);

            if (!string.IsNullOrWhiteSpace(filter.BookTitle))
            {
                var title = filter.BookTitle.Trim().ToLower();
                query = query.Where(ub => ub.Book.Title.ToLower().Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(filter.BookAuthor))
            {
                var author = filter.BookAuthor.Trim().ToLower();
                query = query.Where(ub => ub.Book.Author.ToLower().Contains(author));
            }

            return query;
        }

        private static IQueryable<UserBook> ApplySort(IQueryable<UserBook> query, UserBookQueryObject filter)
        {
            return filter.SortBy switch
            {
                UserBookSortBy.Author => filter.SortDescending ? query.OrderByDescending(ub => ub.Book.Author) : query.OrderBy(ub => ub.Book.Author),
                UserBookSortBy.ReadDate => filter.SortDescending ? query.OrderByDescending(ub => ub.ReadDate) : query.OrderBy(ub => ub.ReadDate),
                UserBookSortBy.UserRating => filter.SortDescending ? query.OrderByDescending(ub => ub.UserRating) : query.OrderBy(ub => ub.UserRating),
                _ => filter.SortDescending ? query.OrderByDescending(ub => ub.Book.Title) : query.OrderBy(ub => ub.Book.Title),
            };
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

        public async Task<UserBook?> UpdateStatusAsync(int userId, int bookId, UserBookUpdateStatusDTO dto)
        {
            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

            if (userBook == null) return null;

            userBook.Status = dto.Status;

            // Automatically manage ReadDate based on status
            if (dto.Status == ReadingStatus.Read)
                userBook.ReadDate = DateTime.UtcNow;
            else
                userBook.ReadDate = null;

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