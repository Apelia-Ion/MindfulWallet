using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly AppDbContext _context;

        public AchievementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAchievementAsync(Achievement achievement)
        {
            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Achievement>> GetAchievementsByUserIdAsync(int userId)
        {
            return await _context.Achievements
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
