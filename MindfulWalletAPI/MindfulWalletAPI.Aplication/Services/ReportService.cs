using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _reportRepository.GetReportByIdAsync(reportId);
        }

        public async Task<IEnumerable<Report>> GetReportsByAccountIdAsync(int accountId)
        {
            return await _reportRepository.GetReportsByAccountIdAsync(accountId);
        }

        public async Task<Report> AddReportAsync(Report report)
        {
            return await _reportRepository.AddReportAsync(report);
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            return await _reportRepository.DeleteReportAsync(reportId);
        }

        public async Task<Report> GetCurrentMonthReportAsync(int accountId)
        {
            return await _reportRepository.GetCurrentMonthReportAsync(accountId);
        }

        public async Task AddOrUpdateReportAsync(Report report)
        {
            var existingReport = await _reportRepository.GetReportAsync(report.AccountId, report.Month);
            if (existingReport != null)
            {
                existingReport.TotalExpenses = report.TotalExpenses;
                existingReport.NumberOfExpenses = report.NumberOfExpenses;
                await _reportRepository.UpdateReportAsync(existingReport);
            }
            else
            {
                await _reportRepository.AddReportAsync(report);
            }
        }
    }
}
