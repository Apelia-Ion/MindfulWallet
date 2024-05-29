using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(User user);

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);

        Task SaveAsync();

        Task<User> GetUserByUsernameAsync(string username);
    }
}
