using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
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

        public async Task<IEnumerable<AccountDto>> GetAccountsByUserIdAsync(int userId)
        {
            var accounts = await _accountRepository.GetAccountsByUserIdAsync(userId);
            return accounts.Select(account => new AccountDto
            {
                Id = account.Id,
                FinanceId = account.FinanceId,
                Type = account.Type,
                Amount = account.Amount
            }).ToList();
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

        public async Task<bool> AddFundsAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account == null) return false;

            account.Amount += amount;
            await _accountRepository.UpdateAsync(account);
            return true;
        }

        public async Task UpdateAccountAmount(int accountId, decimal amountChange)
        {
            var account = await _accountRepository.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                account.Amount += amountChange;
                await _accountRepository.UpdateAsync(account);
            }
        }
    }
}
