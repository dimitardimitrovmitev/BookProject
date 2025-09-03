using BookProject.Models;
using static BookProject.DTOs.BookDTOs;

namespace BookProject.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task<Book?> UpdateBookAsync(int id, BookUpdateDTO bookDto);
        Task<Book?> DeleteBookAsync(int id);
    }
}
