using BookProject.Models;

namespace BookProject.Interfaces
{
    public interface IReportRepository
    {
        Task<List<Report>> GetAllReportsAsync();
        Task<Report?> GetReportByIdAsync(int id);
        Task<Report> CreateReportAsync(Report report);
        Task<Report?> DeleteReportAsync(int id);
    }
}
