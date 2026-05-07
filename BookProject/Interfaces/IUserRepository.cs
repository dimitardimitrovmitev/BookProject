using BookProject.Models;

namespace BookProject.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> PromoteToAdminAsync(int userId);
        Task<User?> DemoteToUserAsync(int userId);
    }
}