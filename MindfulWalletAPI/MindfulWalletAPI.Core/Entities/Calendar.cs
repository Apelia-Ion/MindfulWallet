using MindfulWallet.Core.Entities.Base;
using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.Entities
{
    public class Calendar : BaseEntity

    {
        public int UserId { get; set; }

        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Event> Events { get; set; } = new List<Event>();

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
