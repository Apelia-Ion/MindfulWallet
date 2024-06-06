using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReport(int reportId)
        {
            var report = await _reportService.GetReportByIdAsync(reportId);
            if (report == null)
            {
                return NotFound(new { Message = "Report not found" });
            }
            return Ok(report);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetReportsByAccount(int accountId)
        {
            var reports = await _reportService.GetReportsByAccountIdAsync(accountId);
            if (reports == null || !reports.Any())
            {
                return NotFound(new { Message = "No reports found for the given account." });
            }
            return Ok(reports);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReport = await _reportService.AddReportAsync(report);
            return CreatedAtAction(nameof(GetReport), new { reportId = createdReport.Id }, createdReport);
        }

        [HttpDelete("{reportId}")]
        public async Task<IActionResult> DeleteReport(int reportId)
        {
            var result = await _reportService.DeleteReportAsync(reportId);
            if (!result)
            {
                return NotFound(new { Message = "Report not found" });
            }
            return NoContent();
        }
    }
}
