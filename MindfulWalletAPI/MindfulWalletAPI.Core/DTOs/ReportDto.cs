using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Core.DTOs
{
    public class ReportDto
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public DateTime Month { get; set; }

        [Required]
        public decimal TotalExpenses { get; set; }

        [Required]
        public int NumberOfExpenses { get; set; }
    }

}
