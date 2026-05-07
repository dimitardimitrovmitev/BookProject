using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public static class UserDTOs
    {
        public class UserReadDTO
        {
            public int UserId { get; set; }
            public string Username { get; set; } = null!;
            public string Email { get; set; } = null!;
        }

        public class UserUpdateDTO
        {
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
            public string? Username { get; set; }

            [EmailAddress(ErrorMessage = "A valid email address is required.")]
            [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
            public string? Email { get; set; }

            public string? CurrentPassword { get; set; }

            [MinLength(8, ErrorMessage = "New password must be at least 8 characters.")]
            [StringLength(100, ErrorMessage = "New password cannot exceed 100 characters.")]
            public string? NewPassword { get; set; }
        }
    }
}