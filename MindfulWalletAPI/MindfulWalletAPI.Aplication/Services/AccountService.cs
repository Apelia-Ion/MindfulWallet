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
        private readonly IEventRepository _eventRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IAchievementService _achievementService;
        private readonly IEventService _eventService;

        public AccountService(
            IAccountRepository accountRepository,
            IFinanceRepository financeRepository,
            IEventRepository eventRepository,
            IGoalRepository goalRepository,
            IAchievementService achievementService,
            IEventService eventService)
        {
            _accountRepository = accountRepository;
            _financeRepository = financeRepository;
            _eventRepository = eventRepository;
            _goalRepository = goalRepository;
            _achievementService = achievementService;
            _eventService = eventService;
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
            // Șterge toate evenimentele asociate contului
            var events = await _eventRepository.GetEventsByAccountIdAsync(accountId);
            foreach (var evt in events)
            {
                await _eventRepository.DeleteEventAsync(evt.Id);
            }

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

            // Verifică dacă sunt obiective atinse
            await CheckGoalsAsync(account);

            return true;
        }

        private async Task CheckGoalsAsync(Account account)
        {
            // Verificăm dacă tipul contului este de economii
            if (!account.Type.ToLower().Contains("economii")) return;

            var goals = await _goalRepository.GetGoalsByUserIdAsync(account.Finance.UserId);

            foreach (var goal in goals.Where(g => g.Status == "pending"))
            {
                if (account.Amount >= goal.Amount)
                {
                    goal.Status = "completed";
                    await _goalRepository.UpdateAsync(goal);

                    var achievement = new Achievement
                    {
                        UserId = goal.UserId,
                        Title = $"Achieved goal: {goal.Title}",
                        Description = goal.Description,
                        ImageUrl = "https://cdn1.iconfinder.com/data/icons/seo-and-marketing-icons-2/512/93-512.png", 
                        DateAchieved = DateTime.Now
                    };

                    await _achievementService.AddAchievementAsync(achievement);
                }
            }
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
