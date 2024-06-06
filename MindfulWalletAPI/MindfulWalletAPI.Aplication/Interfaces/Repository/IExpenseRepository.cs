using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IExpenseRepository
    {
        Task<Expense> GetExpenseByIdAsync(int expenseId);
        Task<IEnumerable<Expense>> GetLastThreeExpensesByAccountIdAsync(int accountId);

        Task<IEnumerable<Expense>> GetAllExpensesByAccountIdAsync(int accountId);
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(int expenseId);
    }
}
