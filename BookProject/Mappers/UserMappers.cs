using BookProject.Models;
using static BookProject.DTOs.UserDTOs;

namespace BookProject.Mappers
{
    public static class UserMappers
    {
        public static UserReadDTO ToReadDTO(this User user)
        {
            return new UserReadDTO
            {
                UserId = user.UserId,
                Username = user.Username
            };
        }

        public static UserPrivateReadDTO ToPrivateReadDTO(this User user)
        {
            return new UserPrivateReadDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}