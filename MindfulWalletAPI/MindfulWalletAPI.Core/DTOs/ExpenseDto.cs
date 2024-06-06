using System;
using System.ComponentModel.DataAnnotations;

namespace MindfulWallet.Core.DTOs
{
    public class ExpenseDTO
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}
