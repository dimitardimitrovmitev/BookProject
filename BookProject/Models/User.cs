namespace BookProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
        public ICollection<CharacterReview> Reviews { get; set; } = new List<CharacterReview>();
        public ICollection<Scene> Scenes { get; set; } = new List<Scene>();
        public ICollection<ImageGeneration> Generations { get; set; } = new List<ImageGeneration>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
