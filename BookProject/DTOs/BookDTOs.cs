namespace BookProject.DTOs
{
    public class BookDTOs
    {

        public class BookCreateDTO
        {
            public string Title { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Description { get; set; } = null!;
            public DateTime PublishedDate { get; set; } = DateTime.Now;
            public string OpenLibraryId { get; set; } = null!;
        }
        public class BookReadDTO
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Description { get; set; } = null!;
            public DateTime PublishedDate { get; set; }
            public string OpenLibraryId { get; set; } = null!;
        }

        public class BookOpenLibraryDTO
        {
            public string openlibraryId { get; set; } = null!;
        }

        public class BookUpdateDTO
        {
            public string Title { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Description { get; set; } = null!;
            public DateTime PublishedDate { get; set; } = DateTime.Now;
            public string OpenLibraryId { get; set; } = null!;
        }
    }
}
