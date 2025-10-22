namespace BookProject.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string OpenLibraryId { get; set; } = null!;

        public ICollection<Character> Characters { get; set; } = new List<Character>();

        public ICollection<Scene> Scenes { get; set; } = new List<Scene>();

        public ICollection<BookReview> Reviews { get; set; } = new List<BookReview>();



    }
}
