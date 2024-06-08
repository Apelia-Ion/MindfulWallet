using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository _calendarRepository;

        public CalendarService(ICalendarRepository calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<Calendar> GetOrCreateCalendarByUserIdAsync(int userId)
        {
            var calendar = await _calendarRepository.GetCalendarByUserIdAsync(userId);
            if (calendar == null)
            {
                // Create a new calendar if it doesn't exist
                calendar = new Calendar
                {
                    UserId = userId
                };
                await _calendarRepository.AddCalendarAsync(calendar);
            }
            return calendar;
        }
    }
}
