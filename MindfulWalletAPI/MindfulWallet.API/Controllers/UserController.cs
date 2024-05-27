using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Aplication.Services;
using MindfulWallet.Core.Models;
using MindfulWalletAPI.Models;
using System.Threading.Tasks;

namespace MindfulWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
                return BadRequest(new { Message = "User data is null" });

            var token = await _userService.AuthenticateAsync(loginModel.Email, loginModel.Password);

            if (token == null)
                return BadRequest(new { Message = "Invalid email or password" });

            return Ok(new { message = "Login successful", token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null)
                return BadRequest("User data is null");

            var result = await _userService.RegisterUserAsync(registerModel);

            if (result != "User Registered")
                return BadRequest(new { Message = result });

            return Ok(new { Message = result });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
