using MindfulWallet.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class Report : BaseEntity
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime Month { get; set; }
        public decimal TotalExpenses { get; set; }
        public int NumberOfExpenses { get; set; }
    }
}
