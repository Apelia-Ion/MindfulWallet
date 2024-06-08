using Microsoft.EntityFrameworkCore;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System.Threading.Tasks;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                return false;
            }

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Event> GetEventByExpenseIdAsync(int expenseId) 
        {
            return await _context.Events
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId);
        }

        public async Task<IEnumerable<Event>> GetEventsByAccountIdAsync(int accountId)
        {
            return await _context.Events
                .Where(e => e.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventDto>> GetEventsByUserIdAsync(int userId)
        {
            return await _context.Events
                .Where(e => e.Calendar.UserId == userId)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Date = e.Date,
                    Description = e.Description,
                    Type = e.Type,
                    AccountId = e.AccountId,
                    Amount = e.Amount
                })
                .ToListAsync();
        }


    }
}
