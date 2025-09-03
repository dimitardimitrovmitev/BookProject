namespace BookProject.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }     // reporter
        public string EntityType { get; set; } = null!; // e.g. "Character", "Review"
        public int EntityId { get; set; }   // ID of the reported entity
        public string Reason { get; set; } = null!;
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
        public bool Resolved { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
