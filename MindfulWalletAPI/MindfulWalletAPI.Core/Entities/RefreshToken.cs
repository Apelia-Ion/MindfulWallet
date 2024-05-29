using MindfulWallet.Core.Entities.Base;
using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string? Token { get; set; }
        public DateTime? Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // Foreign key to User
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
