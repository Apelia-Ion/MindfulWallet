using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWallet.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFinanceRepository _financeRepository;

        public AccountService(IAccountRepository accountRepository, IFinanceRepository financeRepository)
        {
            _accountRepository = accountRepository;
            _financeRepository = financeRepository;
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _accountRepository.GetAccountByIdAsync(accountId);
        }

        public async Task<Account> AddAccountAsync(int userId, Account account)
        {
            var finance = await _financeRepository.GetFinanceByUserIdAsync(userId);
            if (finance == null) return null;

            account.FinanceId = finance.Id;
            var createdAccount = await _accountRepository.AddAccountAsync(account);
            return createdAccount;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            return await _accountRepository.DeleteAccountAsync(accountId);
        }
    }
}
