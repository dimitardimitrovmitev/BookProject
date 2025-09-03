using BookProject.DTOs;
using BookProject.Models;
using static BookProject.DTOs.BookDTOs;

namespace BookProject.Mappers
{
    public static class BookMappers
    {
        public static Book ToBookFromCreateDTO(this BookCreateDTO book)
        {
            return new Book
            {
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                PublishedDate = book.PublishedDate,
                OpenLibraryId = book.OpenLibraryId
            };
        }

        public static BookReadDTO ToReadDTO(this Book book)
        {
            return new BookReadDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                PublishedDate = book.PublishedDate,
                OpenLibraryId = book.OpenLibraryId
            };
        }

        public static BookOpenLibraryDTO ToOpenLibraryDTO(this Book book)
        {
            return new BookOpenLibraryDTO
            {
                openlibraryId = book.OpenLibraryId
            };
        }
    }
}
