namespace BookProject.DTOs
{
    public class ReportDTOs
    {
        public class ReportCreateDTO
        {
            public int UserId { get; set; }
            public string EntityType { get; set; } = null!;
            public int EntityId { get; set; }
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