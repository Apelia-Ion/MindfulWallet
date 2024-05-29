using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MindfulWallet.Aplication.Interfaces.Repository;
using MindfulWallet.Aplication.Interfaces.Service;
using MindfulWalletAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MindfulWallet.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, IUserRepository userRepository, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
        }

        public string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
            var identity = new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
        new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var expireMinutesStr = _configuration["JwtConfig:ExpireMinutes"];
            if (!double.TryParse(expireMinutesStr, out double expireMinutes))
            {
                expireMinutes = 60; // Default to 60 minutes if parsing fails
            }

            var currentTime = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = currentTime.AddMinutes(expireMinutes), // Set expiry to 15 seconds for testing
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            // Log the current time and expiry time
            _logger.LogInformation($"Current UTC Time: {currentTime}");
            _logger.LogInformation($"JWT Token Expiry: {tokenDescriptor.Expires}");

            return jwtTokenHandler.WriteToken(token);
        }


        public async Task<string> CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);
            if (tokenInUser != null)
            {
                return await CreateRefreshToken();
            }

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
