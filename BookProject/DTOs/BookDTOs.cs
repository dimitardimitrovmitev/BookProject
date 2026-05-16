using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class BookDTOs
    {
        public class BookCreateDTO
        {
            [Required]
            [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
            public string Title { get; set; } = null!;

            [Required]
            [StringLength(150, MinimumLength = 1, ErrorMessage = "Author must be between 1 and 150 characters.")]
            public string Author { get; set; } = null!;

            [Required]
            [StringLength(2000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 2000 characters.")]
            public string Description { get; set; } = null!;

            public DateTime PublishedDate { get; set; } = DateTime.Now;

            
            [StringLength(50)]
            public string? OpenLibraryId { get; set; }

            public string? CoverUrl { get; set; }

        }

        public class BookReadDTO
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string Author { get; set; } = null!;
            public string Description { get; set; } = null!;
            public DateTime PublishedDate { get; set; }
            public string OpenLibraryId { get; set; } = null!;

            public string? CoverUrl { get; set; } = null!;
        }

        public class BookOpenLibraryDTO
        {
            [Required]
            public string openlibraryId { get; set; } = null!;
        }

        public class BookUpdateDTO
        {
            [Required]
            [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
            public string Title { get; set; } = null!;

            [Required]
            [StringLength(150, MinimumLength = 1, ErrorMessage = "Author must be between 1 and 150 characters.")]
            public string Author { get; set; } = null!;

            [Required]
            [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters.")]
            public string Description { get; set; } = null!;

            public DateTime PublishedDate { get; set; } = DateTime.Now;

            [StringLength(50)]
            public string? OpenLibraryId { get; set; }

            public string? CoverUrl { get; set; }
        }
    }
}