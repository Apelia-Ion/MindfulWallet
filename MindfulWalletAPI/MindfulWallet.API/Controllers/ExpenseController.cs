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

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

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
            return CreatedAtAction(nameof(GetLastThreeExpenses), new { accountId = expense.AccountId }, createdExpense);
        }


        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            var result = await _expenseService.DeleteExpenseAsync(expenseId);
            if (!result)
            {
                return NotFound(new { Message = "Expense not found" });
            }

            return NoContent();
        }
    }
}
