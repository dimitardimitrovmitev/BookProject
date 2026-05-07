using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Interfaces
{
    public interface IUserBookRepository
    {
        Task<PagedResult<UserBook>> GetAllUserBooksAsync(UserBookQueryObject query);
        Task<PagedResult<UserBook>> GetBooksByUserIdAsync(int userId, UserBookQueryObject query);
        Task<UserBook?> GetUserBookAsync(int userId, int bookId);
        Task<UserBook> AddUserBookAsync(UserBook userBook);
        Task<UserBook?> MarkAsReadAsync(int userId, int bookId, UserBookMarkReadDTO dto);
        Task<UserBook?> RateBookAsync(int userId, int bookId, UserBookRateDTO dto);
        Task<UserBook?> RemoveUserBookAsync(int userId, int bookId);
    }
}