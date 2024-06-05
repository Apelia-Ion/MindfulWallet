using MindfulWallet.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class Account : BaseEntity
    {
        public int FinanceId { get; set; }
        public Finance Finance { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }

        // Relație 1-N cu Cheltuieli
        public ICollection<Expense> Expenses { get; set; }
    }
}
