using BookProject.Models;
using static BookProject.DTOs.ReportDTOs;

namespace BookProject.Mappers
{
    public static class ReportMappers
    {
        public static Report ToReportFromCreateDTO(this ReportCreateDTO dto, int userId)
        {
            return new Report
            {
                UserId = userId,
                EntityType = dto.EntityType,
                EntityId = dto.EntityId,
                Reason = dto.Reason,
                ReportedAt = DateTime.UtcNow,
                Resolved = false
            };
        }

        public static ReportReadDTO ToReadDTO(this Report report)
        {
            return new ReportReadDTO
            {
                ReportId = report.ReportId,
                UserId = report.UserId,
                EntityType = report.EntityType,
                EntityId = report.EntityId,
                Reason = report.Reason,
                ReportedAt = report.ReportedAt,
                Resolved = report.Resolved
            };
        }
    }
}