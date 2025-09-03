using BookProject.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookProject.Controllers
{

    [Route("report")]
    public class ReportController : ControllerBase  
    {
        private readonly ApplicationDBContext _context;
        public ReportController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReports()
        {
            var reports = _context.Reports.ToList();
            return Ok(reports);
        }

        [HttpGet("{id}")]

        public IActionResult GetReport([FromRoute] int id)
        {
            var report = _context.Reports.Find(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
    }
}
