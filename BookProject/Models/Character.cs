namespace BookProject.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        public int BookId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;  // hidden from user but used for generation
        public bool Verified { get; set; } = false;

        public Book Book { get; set; } = null!;
        public ICollection<CharacterReview> Reviews { get; set; } = new List<CharacterReview>();
    }
}