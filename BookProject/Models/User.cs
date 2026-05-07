namespace BookProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = "User";

        public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
        public ICollection<CharacterReview> CharacterReviews { get; set; } = new List<CharacterReview>();
        public ICollection<BookReview> BookReviews { get; set; } = new List<BookReview>();
        public ICollection<Scene> Scenes { get; set; } = new List<Scene>();
        public ICollection<ImageGeneration> Generations { get; set; } = new List<ImageGeneration>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
