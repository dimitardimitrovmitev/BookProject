using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class AuthDTOs
    {
        public class RegisterDto
        {
            [Required]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
            public string Username { get; set; } = null!;

            [Required]
            [EmailAddress(ErrorMessage = "A valid email address is required.")]
            [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
            public string Email { get; set; } = null!;

            [Required]
            [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
            [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
            public string Password { get; set; } = null!;
        }

        public class LoginDto
        {
            [Required]
            [EmailAddress(ErrorMessage = "A valid email address is required.")]
            public string Email { get; set; } = null!;

            [Required]
            public string Password { get; set; } = null!;
        }
    }
}