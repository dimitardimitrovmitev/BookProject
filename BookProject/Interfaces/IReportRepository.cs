using BookProject.Helpers;
using BookProject.Models;
using BookProject.QueryObjects;

namespace BookProject.Interfaces
{
    public interface IReportRepository
    {
        Task<PagedResult<Report>> GetAllReportsAsync(ReportQueryObject query);
        Task<Report?> GetReportByIdAsync(int id);
        Task<Report> CreateReportAsync(Report report);
        Task<Report?> DeleteReportAsync(int id, int userId);
        Task<Report?> ResolveReportAsync(int id);

    }
}
