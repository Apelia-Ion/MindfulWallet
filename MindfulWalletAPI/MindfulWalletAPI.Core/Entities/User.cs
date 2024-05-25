using MindfulWallet.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MindfulWalletAPI.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
