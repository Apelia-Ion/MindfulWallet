using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly AppDbContext _context;

        public GoalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Goal> AddGoalAsync(Goal newGoal)
        {
            _context.Goals.Add(newGoal);
            await _context.SaveChangesAsync();
            return newGoal;
        }

        public async Task<bool> DeleteGoalAsync(int goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal == null)
            {
                return false;
            }

            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _context.Goals
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }
    }
}
