using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IFinanceRepository _financeRepository;

        public FinanceService(IFinanceRepository financeRepository)
        {
            _financeRepository = financeRepository;
        }

        public async Task<Finance> GetFinanceByUserIdAsync(int userId)
        {
            var finance = await _financeRepository.GetFinanceByUserIdAsync(userId);
            return finance;
        }
    }
}
