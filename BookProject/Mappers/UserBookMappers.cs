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
                Status = ReadingStatus.WantToRead,
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
                Status = userBook.Status,
                ReadDate = userBook.ReadDate,
                UserRating = userBook.UserRating
            };
        }
    }
}