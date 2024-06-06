using MindfulWallet.Core.Entities.Base;
using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class Finance : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        // Relație 1-N cu Conturi
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public decimal TotalAmount
        {
            get
            {
                return Accounts?.Sum(a => a.Balance) ?? 0;
            }

        }

    }
}
