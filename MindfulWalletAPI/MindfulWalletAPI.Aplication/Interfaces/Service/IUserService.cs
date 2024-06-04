using MindfulWallet.Core.DTOs;
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
        Task<TokenApiDto> AuthenticateAsync(string email, string password);
        Task<string> RegisterUserAsync(RegisterModel registerModel);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<TokenApiDto> RefreshTokenAsync(TokenApiDto tokenApiDto);

        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
