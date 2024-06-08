using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Event> AddEventAsync(Event newEvent)
        {
            return await _eventRepository.AddEventAsync(newEvent);
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            return await _eventRepository.DeleteEventAsync(eventId);
        }

        public async Task<Event> GetEventByExpenseIdAsync(int expenseId)
        {
            return await _eventRepository.GetEventByExpenseIdAsync(expenseId);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByUserIdAsync(int userId)
        {
            return await _eventRepository.GetEventsByUserIdAsync(userId);
        }
    }
}
