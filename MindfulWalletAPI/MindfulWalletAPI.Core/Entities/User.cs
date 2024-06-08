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

        //1-N Tokens 
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<ResetToken> ResetTokens { get; set; } = new List<ResetToken>();

        // Relarie 1-1 cu Finante
        public Finance Finance { get; set; }

        // Relatie 1-1 cu Calendar
        public Calendar Calendar { get; set; }

        // Relatii 1-N
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    }

}

