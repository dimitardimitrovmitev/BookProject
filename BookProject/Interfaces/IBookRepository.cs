using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;
using static BookProject.DTOs.BookDTOs;

namespace BookProject.Interfaces
{
    public interface IBookRepository
    {
        Task<PagedResult<Book>> GetAllBooksAsync(BookQueryObject query);
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book?> UpdateBookAsync(int id, BookUpdateDTO bookDto);
        Task<Book?> DeleteBookAsync(int id);
    }
}