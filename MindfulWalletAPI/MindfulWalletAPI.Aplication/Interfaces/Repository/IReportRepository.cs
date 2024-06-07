using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IReportRepository
    {
        Task<Report> GetReportByIdAsync(int reportId);
        Task<IEnumerable<Report>> GetReportsByAccountIdAsync(int accountId);
        Task<Report> AddReportAsync(Report report);
        Task<bool> DeleteReportAsync(int reportId);
        Task<Report> GetCurrentMonthReportAsync(int accountId);

        Task<bool> ReportExistsAsync(int accountId, DateTime month);

        Task<Report> GetReportAsync(int accountId, DateTime month);

        Task UpdateReportAsync(Report report);
    }  
}
