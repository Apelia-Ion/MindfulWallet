using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using System.Threading.Tasks;

namespace MindfulWallet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IAccountService _accountService;

        public ExpenseController(IExpenseService expenseService, IAccountService accountService)
        {
            _expenseService = expenseService;
            _accountService = accountService;
        }

        // Endpoint pentru obținerea ultimelor 3 cheltuieli pentru un cont
        [HttpGet("lastThree/{accountId}")]
        public async Task<IActionResult> GetLastThreeExpenses(int accountId)
        {
            var expenses = await _expenseService.GetLastThreeExpensesByAccountIdAsync(accountId);
            if (expenses == null || !expenses.Any())
            {
                return NotFound(new { Message = "No expenses found for the given account." });
            }
            return Ok(expenses);
        }

        // Endpoint pentru adăugarea unei noi cheltuieli în cont și actualizarea sumei disponibile
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDTO expenseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expense = new Expense
            {
                AccountId = expenseDto.AccountId,
                Amount = expenseDto.Amount,
                Date = expenseDto.Date,
                Description = expenseDto.Description
            };

            var createdExpense = await _expenseService.AddExpenseAsync(expense);
           // await _accountService.UpdateAccountBalance(expense.AccountId, -expense.Amount); // Scade suma cheltuielii din cont
            return CreatedAtAction(nameof(GetLastThreeExpenses), new { accountId = expense.AccountId }, createdExpense);
        }

        // Endpoint pentru ștergerea unei cheltuieli și actualizarea sumei disponibile în cont
        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(expenseId);
            if (expense == null)
            {
                return NotFound(new { Message = "Expense not found" });
            }

            var result = await _expenseService.DeleteExpenseAsync(expenseId);
            if (result)
            {
                await _accountService.UpdateAccountBalance(expense.AccountId, expense.Amount); // Adaugă suma cheltuielii înapoi în cont
                return NoContent();
            }

            return BadRequest(new { Message = "Error deleting expense" });
        }
    }
}
