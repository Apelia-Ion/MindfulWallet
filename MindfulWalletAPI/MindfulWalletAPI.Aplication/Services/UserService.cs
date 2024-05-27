using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.Models;
using MindfulWalletAPI.Helpers;
using MindfulWalletAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                return null;

            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryveryverysecretsecret........");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name,$"{user.Name}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

            
        }

        public async Task<string> RegisterUserAsync(RegisterModel registerModel)
        {
            if (await _userRepository.UsernameExistsAsync(registerModel.UserName))
                return "Username already exists";

            if (await _userRepository.EmailExistsAsync(registerModel.Email))
                return "Email is already linked to an existing account";

            var user = new User
            {
                Name = registerModel.Name,
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                Password = PasswordHasher.HashPassword(registerModel.Password),
                Role ="user",
                Token = ""
            };

            await _userRepository.AddUserAsync(user);

            return "User Registered";
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }



    }
}

