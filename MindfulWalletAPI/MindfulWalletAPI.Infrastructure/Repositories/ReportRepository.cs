using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report> GetReportByIdAsync(int reportId)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public async Task<IEnumerable<Report>> GetReportsByAccountIdAsync(int accountId)
        {
            return await _context.Reports
                .Where(r => r.AccountId == accountId)
                .OrderByDescending(r => r.Month)
                .ToListAsync();
        }

        public async Task<Report> AddReportAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<bool> DeleteReportAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
            {
                return false;
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Report> GetCurrentMonthReportAsync(int accountId)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.AccountId == accountId && r.Month.Month == currentMonth && r.Month.Year == currentYear);
        }

        public async Task<bool> ReportExistsAsync(int accountId, DateTime month)
        {
            return await _context.Reports
                .AnyAsync(r => r.AccountId == accountId && r.Month == month);
        }

        public async Task<Report> GetReportAsync(int accountId, DateTime month)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.AccountId == accountId && r.Month == month);
        }

        public async Task UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }
    }
}
