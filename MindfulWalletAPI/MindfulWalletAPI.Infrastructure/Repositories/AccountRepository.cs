using Microsoft.EntityFrameworkCore;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindfulWallet.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Finance)
                    .ThenInclude(f => f.User)
                        .ThenInclude(u => u.Calendar) // Asigură-te că include Calendar
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _context.Accounts
                .Include(a => a.Finance)
                .Where(a => a.Finance.UserId == userId)
                .ToListAsync();
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
