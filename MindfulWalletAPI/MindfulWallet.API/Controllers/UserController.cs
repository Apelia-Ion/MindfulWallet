using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Application.Services;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }



        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
                return BadRequest(new { Message = "User data is null" });

            var token = await _userService.AuthenticateAsync(loginModel.Email, loginModel.Password);

            if (token == null)
                return BadRequest(new { Message = "Invalid email or password" });

            return Ok(new {
               
                message = "Login successful",
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken

            });
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



        [Authorize]
        [HttpGet("getUserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(user);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto == null)
                return BadRequest("Invalid client request");
            try
            {
                var newToken = await _userService.RefreshTokenAsync(tokenApiDto);
                return Ok(new
                {

                    message = "Refresh successful",
                    AccessToken = newToken.AccessToken,
                    RefreshToken = newToken.RefreshToken
                });

            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token");
            }
        }
    }

}
