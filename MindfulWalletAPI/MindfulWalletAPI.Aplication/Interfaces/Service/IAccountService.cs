using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(int accountId);
        Task<Account> AddAccountAsync(int userId, Account account);
        Task<bool> DeleteAccountAsync(int accountId);
        Task<bool> AddFundsAsync(int accountId, decimal amount);

        Task UpdateAccountAmount(int accountId, decimal amountChange);
    }
}
