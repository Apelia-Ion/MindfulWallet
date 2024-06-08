using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWallet.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFinanceRepository _financeRepository;
        private readonly IEventService _eventService; // Adăugăm IEventService

        public AccountService(IAccountRepository accountRepository, IFinanceRepository financeRepository, IEventService eventService)
        {
            _accountRepository = accountRepository;
            _financeRepository = financeRepository;
            _eventService = eventService; // Inițializăm IEventService
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
            if (account == null || account.Finance == null || account.Finance.User == null || account.Finance.User.Calendar == null)
                return false;

            account.Amount += amount;
            await _accountRepository.UpdateAsync(account);

            // Creăm un nou Event pentru adăugarea fondurilor
            var newEvent = new Event
            {
                CalendarId = account.Finance.User.Calendar.Id, // Asigură-te că entitățile sunt încărcate corect
                Date = DateTime.Now,
                Description = "Added funds",
                Type = "deposit",
                AccountId = accountId,
                Amount = amount
            };

            await _eventService.AddEventAsync(newEvent);

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
