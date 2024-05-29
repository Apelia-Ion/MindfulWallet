using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        Task<string> CreateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
