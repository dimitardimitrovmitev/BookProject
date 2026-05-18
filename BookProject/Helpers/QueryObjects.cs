using BookProject.Helpers;
using BookProject.Models;

namespace BookProject.QueryObjects
{
    public enum BookSortBy
    {
        Title,
        Author,
        PublishedDate
    }

    public enum CharacterSortBy
    {
        Name,
        Verified,
        BookTitle
    }

    public enum ImageGenerationSortBy
    {
        GeneratedAt
    }

    public enum BookReviewSortBy
    {
        CreatedAt,
        Rating
    }

    public enum CharacterReviewSortBy
    {
        CreatedAt,
        Rating
    }

    public enum SceneSortBy
    {
        CreatedAt,
        Title
    }

    public enum UserBookSortBy
    {
        Title,
        Author,
        ReadDate
    }

    public enum ReportSortBy
    {
        ReportedAt
    }

    public class BookQueryObject : QueryObject
    {
        public string? Search { get; set; }
        public DateTime? PublishedAfter { get; set; }
        public BookSortBy SortBy { get; set; } = BookSortBy.Title;
    }

    public class CharacterQueryObject : QueryObject
    {
        public string? Search { get; set; }
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public bool? Verified { get; set; }
        public CharacterSortBy SortBy { get; set; } = CharacterSortBy.Name;
    }

    public class ImageGenerationQueryObject : QueryObject
    {
        private int _pageSize = 24;

        public new int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }

        public int? UserId { get; set; }
        public int? SceneId { get; set; }
        public int? CharacterId { get; set; }
        public string? CharacterName { get; set; }
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public ImageGenerationSortBy SortBy { get; set; } = ImageGenerationSortBy.GeneratedAt;
    }

    public class BookReviewQueryObject : QueryObject
    {
        public int? BookId { get; set; }
        public string? BookTitle { get; set; }
        public int? UserId { get; set; }
        public BookReviewSortBy SortBy { get; set; } = BookReviewSortBy.CreatedAt;
    }

    public class CharacterReviewQueryObject : QueryObject
    {
        public int? CharacterId { get; set; }
        public string? CharacterName { get; set; }
        public int? UserId { get; set; }
        public CharacterReviewSortBy SortBy { get; set; } = CharacterReviewSortBy.CreatedAt;
    }

    public class SceneQueryObject : QueryObject
    {
        public int? UserId { get; set; }
        public string? CharacterName { get; set; }
        public SceneSortBy SortBy { get; set; } = SceneSortBy.CreatedAt;
    }

    public class UserBookQueryObject : QueryObject
    {
        public ReadingStatus? Status { get; set; }
        public string? BookTitle { get; set; }
        public string? BookAuthor { get; set; }
        public UserBookSortBy SortBy { get; set; } = UserBookSortBy.Title;
    }

    public class ReportQueryObject : QueryObject
    {
        public string? EntityType { get; set; }
        public bool? Resolved { get; set; }
        public ReportSortBy SortBy { get; set; } = ReportSortBy.ReportedAt;
    }
}