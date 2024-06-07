using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> CreateOrUpdateReport([FromBody] ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = new Report
            {
                AccountId = reportDto.AccountId,
                Month = reportDto.Month,
                TotalExpenses = reportDto.TotalExpenses,
                NumberOfExpenses = reportDto.NumberOfExpenses
            };

            await _reportService.AddOrUpdateReportAsync(report);
            return Ok(report);
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

        [HttpGet("current/{accountId}")]
        public async Task<IActionResult> GetCurrentMonthReport(int accountId)
        {
            var report = await _reportService.GetCurrentMonthReportAsync(accountId);
            if (report == null)
            {
                return NotFound(new { Message = "Current month report not found" });
            }
            return Ok(report);
        }
    }
}
