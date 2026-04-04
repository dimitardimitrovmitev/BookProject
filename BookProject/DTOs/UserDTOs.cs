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
    }
}