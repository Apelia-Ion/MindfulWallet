using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IEventService _eventService; // Adăugăm IEventService

        public ExpenseService(IExpenseRepository expenseRepository, IEventService eventService)
        {
            _expenseRepository = expenseRepository;
            _eventService = eventService; // Inițializăm IEventService
        }

        public async Task<Expense> GetExpenseByIdAsync(int expenseId)
        {
            return await _expenseRepository.GetExpenseByIdAsync(expenseId);
        }

        public async Task<IEnumerable<Expense>> GetLastThreeExpensesByAccountIdAsync(int accountId)
        {
            return await _expenseRepository.GetLastThreeExpensesByAccountIdAsync(accountId);
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesByAccountIdAsync(int accountId)
        {
            return await _expenseRepository.GetAllExpensesByAccountIdAsync(accountId);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            var addedExpense = await _expenseRepository.AddExpenseAsync(expense);

            // Asigură-te că referințele sunt încărcate corect
            var calendar = addedExpense.Account.Finance.User.Calendar;

            if (calendar == null)
            {
                throw new Exception("Calendar not found");
            }

            var newEvent = new Event
            {
                CalendarId = calendar.Id,
                Date = addedExpense.Date,
                Description = addedExpense.Description,
                Type = "withdrawal",
                AccountId = addedExpense.AccountId,
                ExpenseId = addedExpense.Id,
                Amount = addedExpense.Amount
            };

            await _eventService.AddEventAsync(newEvent);

            return addedExpense;
        }


        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(expenseId);
            if (expense == null)
            {
                return false;
            }

            // Găsim și ștergem eventul asociat cu cheltuiala
            var associatedEvent = await _eventService.GetEventByExpenseIdAsync(expenseId);
            if (associatedEvent != null)
            {
                await _eventService.DeleteEventAsync(associatedEvent.Id);
            }

            return await _expenseRepository.DeleteExpenseAsync(expenseId);
        }
    }
}
