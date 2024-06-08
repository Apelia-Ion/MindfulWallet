using MindfulWallet.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IGoalRepository
    {
        Task<Goal> AddGoalAsync(Goal newGoal);
        Task<bool> DeleteGoalAsync(int goalId);
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId);

        Task UpdateAsync(Goal goal); 
    }
}
