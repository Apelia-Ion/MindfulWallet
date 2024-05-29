using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWallet.Core.DTOs;
using MindfulWallet.Core.Entities;
using MindfulWallet.Core.Models;
using MindfulWalletAPI.Helpers;
using MindfulWalletAPI.Models;

namespace MindfulWallet.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;

        public UserService(IUserRepository userRepository, ITokenService tokenService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<TokenApiDto> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
                return null;

            var accessToken = _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(); //string

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7), //standard
                UserId = user.Id
            };

            user.RefreshTokens.Add(refreshTokenEntity);
            await _userRepository.SaveAsync();

            return new TokenApiDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
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
                Role = "user"
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


        public async Task<TokenApiDto> RefreshTokenAsync(TokenApiDto tokenApiDto)
        {
            // Add logging here
            _logger.LogInformation("Attempting to refresh token...");

            if (tokenApiDto == null)
                throw new ArgumentNullException(nameof(tokenApiDto));

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiDto.AccessToken);
            var username = principal.Identity.Name;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || user.RefreshTokens.All(rt => rt.Token != tokenApiDto.RefreshToken || rt.Expires <= DateTime.Now))
            {
                _logger.LogWarning("Invalid or expired refresh token");
                throw new SecurityTokenException("Invalid token");
            }

            var newAccessToken = _tokenService.GenerateJwtToken(user);
            var newRefreshToken = await _tokenService.CreateRefreshToken();
            var currentTime = DateTime.UtcNow;

            var refreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            user.RefreshTokens.Add(refreshTokenEntity);
            await _userRepository.SaveAsync();

            // Log new token details
            _logger.LogInformation($"Current UTC Time: {currentTime}");
            _logger.LogInformation($"New Access Token: {newAccessToken}, New Refresh Token: {newRefreshToken}");

            return new TokenApiDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }






    }

}
