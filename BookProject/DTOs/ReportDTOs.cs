using System.ComponentModel.DataAnnotations;

namespace BookProject.DTOs
{
    public class ReportDTOs
    {
        public class ReportCreateDTO
        {
            [Required]
            [StringLength(50, MinimumLength = 1, ErrorMessage = "EntityType must be between 1 and 50 characters.")]
            public string EntityType { get; set; } = null!;

            [Range(1, int.MaxValue, ErrorMessage = "A valid EntityId is required.")]
            public int EntityId { get; set; }

            [Required]
            [StringLength(500, MinimumLength = 5, ErrorMessage = "Reason must be between 5 and 500 characters.")]
            public string Reason { get; set; } = null!;
        }

        public class ReportReadDTO
        {
            public int ReportId { get; set; }
            public int UserId { get; set; }
            public string EntityType { get; set; } = null!;
            public int EntityId { get; set; }
            public string Reason { get; set; } = null!;
            public DateTime ReportedAt { get; set; }
            public bool Resolved { get; set; }
        }
    }
}