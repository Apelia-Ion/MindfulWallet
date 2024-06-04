using MindfulWallet.Core.Entities;
using MindfulWallet.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace MindfulWalletAPI.Models
{
    public class User : BaseEntity
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<ResetToken> ResetTokens { get; set; } = new List<ResetToken>();
    }
}
