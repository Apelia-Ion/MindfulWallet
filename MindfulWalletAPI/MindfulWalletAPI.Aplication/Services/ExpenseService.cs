using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<Expense> GetExpenseByIdAsync(int expenseId)
        {
            return await _expenseRepository.GetExpenseByIdAsync(expenseId);
        }

        public async Task<IEnumerable<Expense>> GetLastThreeExpensesByAccountIdAsync(int accountId)
        {
            return await _expenseRepository.GetLastThreeExpensesByAccountIdAsync(accountId);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            return await _expenseRepository.AddExpenseAsync(expense);
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            return await _expenseRepository.DeleteExpenseAsync(expenseId);
        }
    }
}
