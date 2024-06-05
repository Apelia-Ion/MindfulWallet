using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Core.Entities;
using MindfulWallet.Aplication.Interfaces.Service;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IFinanceService _financeService;

        public FinanceController(IFinanceService financeService)
        {
            _financeService = financeService;
        }

        // Endpoint pentru obținerea listei de conturi ale utilizatorului
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFinance(int userId)
        {
            var finance = await _financeService.GetFinanceByUserIdAsync(userId);
            if (finance == null)
            {
                return NotFound(new { Message = "Finance not found for the given user." });
            }
            return Ok(finance.Accounts);
        }
    }
}
