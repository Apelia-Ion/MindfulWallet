using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Application.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementService(IAchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        public async Task AddAchievementAsync(Achievement achievement)
        {
            await _achievementRepository.AddAchievementAsync(achievement);
        }

        public async Task<IEnumerable<Achievement>> GetAchievementsByUserIdAsync(int userId)
        {
            return await _achievementRepository.GetAchievementsByUserIdAsync(userId);
        }
    }
}
