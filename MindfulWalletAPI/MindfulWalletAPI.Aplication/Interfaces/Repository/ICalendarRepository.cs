using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface ICalendarRepository
    {
        Task<Calendar> GetCalendarByUserIdAsync(int userId);
        Task<Calendar> AddCalendarAsync(Calendar calendar);
    }
}
