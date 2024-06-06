using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;

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

        // Endpoint pentru adaugarea unei noi cheltuieli in cont

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

        // Endpoint pentru stergerea unei cheltuieli

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
