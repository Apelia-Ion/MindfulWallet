using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Service
{
    public interface IFinanceService
    {
        Task<Finance> GetFinanceByUserIdAsync(int userId);
    }
}
