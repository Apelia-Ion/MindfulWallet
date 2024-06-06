using MindfulWallet.Core.Entities.Base;
using MindfulWallet.Core.Entities;

public class Account : BaseEntity
{
    public int FinanceId { get; set; }
    public Finance Finance { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }

    // Relație 1-N cu Cheltuieli
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public decimal Balance
    {
        get
        {
            return Amount - (Expenses?.Sum(e => e.Amount)) ?? 0;
        }
    }
}
