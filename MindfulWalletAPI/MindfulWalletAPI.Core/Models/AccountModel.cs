using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Models
{
    public class AccountModel
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}

