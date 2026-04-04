using BookProject.Models;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Interfaces
{
    public interface IUserBookRepository
    {
        Task<List<UserBook>> GetAllUserBooksAsync();
        Task<List<UserBook>> GetBooksByUserIdAsync(int userId);
        Task<UserBook?> GetUserBookAsync(int userId, int bookId);
        Task<UserBook> AddUserBookAsync(UserBook userBook);
        Task<UserBook?> MarkAsReadAsync(int userId, int bookId, UserBookMarkReadDTO dto);
        Task<UserBook?> RateBookAsync(int userId, int bookId, UserBookRateDTO dto);
        Task<UserBook?> RemoveUserBookAsync(int userId, int bookId);
    }
}