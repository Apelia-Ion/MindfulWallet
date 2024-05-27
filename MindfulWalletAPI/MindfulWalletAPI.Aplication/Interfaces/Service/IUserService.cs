using MindfulWallet.Core.Models;
using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string email, string password);
        Task<string> RegisterUserAsync(RegisterModel registerModel);

        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
