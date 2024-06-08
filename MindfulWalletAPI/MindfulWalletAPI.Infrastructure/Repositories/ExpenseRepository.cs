using Microsoft.EntityFrameworkCore;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Expense> GetExpenseByIdAsync(int expenseId)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId);
        }

        public async Task<IEnumerable<Expense>> GetLastThreeExpensesByAccountIdAsync(int accountId)
        {
            return await _context.Expenses
                .Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.Date)
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesByAccountIdAsync(int accountId)  // Adaugă această metodă
        {
            return await _context.Expenses
                .Where(e => e.AccountId == accountId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            var account = await _context.Accounts
                                        .Include(a => a.Finance)
                                        .ThenInclude(f => f.User)
                                        .ThenInclude(u => u.Calendar)
                                        .FirstOrDefaultAsync(a => a.Id == expense.AccountId);

            if (account == null)
            {
                throw new Exception("Account not found");
            }

            expense.Account = account;

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId)
        {
            var expense = await _context.Expenses.FindAsync(expenseId);
            if (expense == null)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}