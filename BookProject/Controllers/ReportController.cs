using BookProject.Data;
using BookProject.Interfaces;
using BookProject.Models;
using Microsoft.AspNetCore.Mvc;
using static BookProject.DTOs.ReportDTOs;
using BookProject.Mappers;

namespace BookProject.Controllers
{

    [Route("report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepo;

        public ReportController(IReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportRepo.GetAllReportsAsync();
            return Ok(reports.Select(r => r.ToReadDTO()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(int id)
        {
            var report = await _reportRepo.GetReportByIdAsync(id);
            if (report == null) return NotFound();

            return Ok(report.ToReadDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDTO dto)
        {
            var reportModel = dto.ToReportFromCreateDTO();

            var created = await _reportRepo.CreateReportAsync(reportModel);

            return CreatedAtAction(nameof(GetReport),
                new { id = created.ReportId },
                created.ToReadDTO());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var deleted = await _reportRepo.DeleteReportAsync(id);

            if (deleted == null) return NotFound();

            return NoContent();
        }
    }
}
