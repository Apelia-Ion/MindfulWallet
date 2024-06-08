using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IAchievementService
    {
        Task AddAchievementAsync(Achievement achievement);
        Task<IEnumerable<Achievement>> GetAchievementsByUserIdAsync(int userId);
    }
}
