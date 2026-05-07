using BookProject.Data;
using BookProject.Helpers;
using BookProject.Interfaces;
using BookProject.Models;
using BookProject.QueryObjects;
using Microsoft.EntityFrameworkCore;

namespace BookProject.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDBContext _context;

        public ReportRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Report>> GetAllReportsAsync(ReportQueryObject query)
        {
            var reports = _context.Reports.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.EntityType))
                reports = reports.Where(r => r.EntityType == query.EntityType);

            if (query.Resolved.HasValue)
                reports = reports.Where(r => r.Resolved == query.Resolved.Value);

            reports = query.SortDescending
                ? reports.OrderByDescending(r => r.ReportedAt)
                : reports.OrderBy(r => r.ReportedAt);

            var totalCount = await reports.CountAsync();

            var items = await reports
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Report>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<Report?> GetReportByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<Report> CreateReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<Report?> DeleteReportAsync(int id, int userId)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == id && r.UserId == userId);

            if (report == null) return null;

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<Report?> ResolveReportAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null) return null;

            report.Resolved = true;
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
