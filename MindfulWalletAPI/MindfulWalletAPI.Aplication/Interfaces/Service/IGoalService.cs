using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IGoalService
    {
        Task<Goal> AddGoalAsync(Goal newGoal);
        Task<bool> DeleteGoalAsync(int goalId);
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId);
    }
}
