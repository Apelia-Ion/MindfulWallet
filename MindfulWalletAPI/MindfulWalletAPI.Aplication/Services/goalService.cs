using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;

        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<Goal> AddGoalAsync(Goal newGoal)
        {
            return await _goalRepository.AddGoalAsync(newGoal);
        }

        public async Task<bool> DeleteGoalAsync(int goalId)
        {
            return await _goalRepository.DeleteGoalAsync(goalId);
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            return await _goalRepository.GetGoalsByUserIdAsync(userId);
        }
    }
}
