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
    public class Achievement : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }          
        public string Description { get; set; }    
        public string ImageUrl { get; set; }       
        public DateTime DateAchieved { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
