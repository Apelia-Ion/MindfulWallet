using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly AppDbContext _context;

        public CalendarRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Calendar> GetCalendarByUserIdAsync(int userId)
        {
            return await _context.Calendars.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Calendar> AddCalendarAsync(Calendar calendar)
        {
            _context.Calendars.Add(calendar);
            await _context.SaveChangesAsync();
            return calendar;
        }
    }
}
