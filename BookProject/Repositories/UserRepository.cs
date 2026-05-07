using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> PromoteToAdminAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return null;

            user.Role = "Admin";
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DemoteToUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return null;

            user.Role = "User";
            await _context.SaveChangesAsync();
            return user;
        }
    }
}