using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountByIdAsync(int accountId);
        Task<Account> AddAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int accountId);

        Task UpdateAsync(Account account);
    }
}
