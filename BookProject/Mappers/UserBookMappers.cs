using BookProject.Models;
using static BookProject.DTOs.UserBookDTOs;

namespace BookProject.Mappers
{
    public static class UserBookMappers
    {
        public static UserBook ToUserBookFromCreateDTO(this UserBookCreateDTO dto, int userId)
        {
            return new UserBook
            {
                UserId = userId,
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
                UserId = userBook.UserId,
                BookId = userBook.BookId,
                IsRead = userBook.IsRead,
                ReadDate = userBook.ReadDate,
                UserRating = userBook.UserRating
            };
        }
    }
}