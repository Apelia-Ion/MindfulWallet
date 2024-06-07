using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Aplication.Services;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.DTOs.MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using MindfulWallet.Core.Models;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IExpenseService _expenseService;

        public AccountController(IAccountService accountService, IExpenseService expenseService)
        {
            _accountService = accountService;
            _expenseService = expenseService;
        }

        // Endpoint pentru obținerea unui cont cu ultimele 3 cheltuieli
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return NotFound(new { Message = "Account not found" });
            }

            // Păstrează doar ultimele 3 cheltuieli
            account.Expenses = account.Expenses
                .OrderByDescending(e => e.Date)
                .Take(3)
                .ToList();

            return Ok(account);
        }

        [HttpGet("{accountId}/expenses")]
        public async Task<IActionResult> GetAllExpenses(int accountId)
        {
            var expenses = await _expenseService.GetAllExpensesByAccountIdAsync(accountId);
            if (expenses == null || !expenses.Any())
            {
                return NotFound(new { Message = "No expenses found for the given account." });
            }
            return Ok(expenses);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAccountsByUserId(int userId)
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(userId);
            if (accounts == null || !accounts.Any())
            {
                return NotFound(new { Message = "No accounts found for the given user." });
            }
            return Ok(accounts);
        }


        // Endpoint pentru adăugarea unui cont
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateAccount(int userId, [FromBody] AccountModel accountModel)
        {
            var account = new Account
            {
                Type = accountModel.Type,
                Amount = accountModel.Amount
            };

            var createdAccount = await _accountService.AddAccountAsync(userId, account);
            if (createdAccount == null)
            {
                return BadRequest(new { Message = "Failed to create account" });
            }
            return Ok(new { Message = createdAccount });
        }

        // Endpoint pentru ștergerea unui cont
        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result = await _accountService.DeleteAccountAsync(accountId);
            if (!result)
            {
                return NotFound(new { Message = "Account not found" });
            }
            return NoContent();
        }

        // Endpoint pentru adăugarea banilor într-un cont
        [HttpPost("addFunds")]
        public async Task<IActionResult> AddFunds([FromBody] AddFundsDto addFundsDto)
        {
            var result = await _accountService.AddFundsAsync(addFundsDto.AccountId, addFundsDto.Amount);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to add funds" });
            }
            return Ok(new { Message = "Funds added successfully" });
        }
    }
}
