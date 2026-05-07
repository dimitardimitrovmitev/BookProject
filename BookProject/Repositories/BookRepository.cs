using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
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

        public async Task<PagedResult<Book>> GetAllBooksAsync(BookQueryObject query)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                books = books.Where(b =>
                    b.Title.ToLower().Contains(search) ||
                    b.Author.ToLower().Contains(search));
            }

            if (query.PublishedAfter.HasValue)
                books = books.Where(b => b.PublishedDate >= query.PublishedAfter.Value);

            books = query.SortBy switch
            {
                BookSortBy.Author => query.SortDescending ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
                BookSortBy.PublishedDate => query.SortDescending ? books.OrderByDescending(b => b.PublishedDate) : books.OrderBy(b => b.PublishedDate),
                _ => query.SortDescending ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
            };

            var totalCount = await books.CountAsync();

            var items = await books
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Book>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateBookAsync(int id, BookUpdateDTO bookDto)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBook == null) return null;

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.Description = bookDto.Description;
            existingBook.PublishedDate = bookDto.PublishedDate;
            existingBook.OpenLibraryId = bookDto.OpenLibraryId;

            await _context.SaveChangesAsync();
            return existingBook;
        }

        public async Task<Book?> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null) return null;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}