using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IEventRepository
    {
        Task<Event> AddEventAsync(Event newEvent);
        Task<bool> DeleteEventAsync(int eventId);
        Task<Event> GetEventByExpenseIdAsync(int expenseId);
        Task<IEnumerable<EventDto>> GetEventsByUserIdAsync(int userId);
    }
}
