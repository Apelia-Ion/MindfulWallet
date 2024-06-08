using MindfulWallet.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class Event : BaseEntity
    {
        public int CalendarId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // deposit, withdrawal
        public int? AccountId { get; set; }
        public int? ExpenseId { get; set; }
        public decimal Amount { get; set; }

        public Calendar Calendar { get; set; }
        public Account Account { get; set; }
    }
}
