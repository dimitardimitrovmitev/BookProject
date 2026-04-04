using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
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

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _context.Reports.ToListAsync();
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

        public async Task<Report?> DeleteReportAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) return null;

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
