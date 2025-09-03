using BookProject.Data;
using BookProject.DTOs;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.EntityFrameworkCore;
using static BookProject.DTOs.BookDTOs;

namespace BookProject.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _context;
        public BookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if(book == null) return null;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book?> UpdateBookAsync(int id, BookUpdateDTO bookDto)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if(existingBook == null) return null;

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.Description = bookDto.Description;
            existingBook.PublishedDate = bookDto.PublishedDate;
            existingBook.OpenLibraryId = bookDto.OpenLibraryId;

            await _context.SaveChangesAsync();

            return existingBook;
        }
    }
}
