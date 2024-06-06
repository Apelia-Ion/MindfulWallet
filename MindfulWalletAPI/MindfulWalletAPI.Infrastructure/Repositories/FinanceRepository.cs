using Microsoft.EntityFrameworkCore;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Core.Entities;
using MindfulWalletAPI.Context;
using System.Threading.Tasks;

public class FinanceRepository : IFinanceRepository
{
    private readonly AppDbContext _context;

    public FinanceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Finance> GetFinanceByUserIdAsync(int userId)
    {
        return await _context.Finances
            .Include(f => f.Accounts)
                .ThenInclude(a => a.Expenses)
            .FirstOrDefaultAsync(f => f.UserId == userId);
    }


    public async Task<Finance> AddFinanceAsync(Finance finance)
    {
        _context.Finances.Add(finance);
        await _context.SaveChangesAsync();
        return finance;
    }
}
