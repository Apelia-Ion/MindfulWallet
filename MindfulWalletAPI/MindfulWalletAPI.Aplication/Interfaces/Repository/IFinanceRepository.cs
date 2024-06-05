using MindfulWallet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Interfaces.Repository
{
    public interface IFinanceRepository
    {
        Task<Finance> GetFinanceByUserIdAsync(int userId);
        Task<Finance> AddFinanceAsync(Finance finance);
    }
}
