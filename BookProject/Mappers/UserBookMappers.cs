using BookProject.Models;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Mappers
{
    public static class UserBookMappers
    {
        public static UserBook ToUserBookFromCreateDTO(this UserBookCreateDTO dto)
        {
            return new UserBook
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                IsRead = false,
                ReadDate = null,
                UserRating = null
            };
        }

        public static UserBookReadDTO ToReadDTO(this UserBook userBook)
        {
            return new UserBookReadDTO
            {
                UserBookId = userBook.UserBookId,
                UserId = userBook.UserId,
                BookId = userBook.BookId,
                IsRead = userBook.IsRead,
                ReadDate = userBook.ReadDate,
                UserRating = userBook.UserRating
            };
        }
    }
}