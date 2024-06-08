using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IEventService
    {
        Task<Event> AddEventAsync(Event newEvent);
        Task<bool> DeleteEventAsync(int eventId);
        Task<Event> GetEventByExpenseIdAsync(int expenseId);
        Task<IEnumerable<EventDto>> GetEventsByUserIdAsync(int userId);
    }
}
