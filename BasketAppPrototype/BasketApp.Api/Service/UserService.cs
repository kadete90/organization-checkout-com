using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BasketApp.Api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasketApp.Api.Service
{
    public interface IAccountService
    {
        string GenerateToken(IdentityUser user);
    }

    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IOptions<AppSettings> appSettings, ILogger<AccountService> looger)
        {
            _appSettings = appSettings.Value;
            _logger = looger;
        }

        public string GenerateToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var signingKey = new SymmetricSecurityKey(secret);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: DateTime.UtcNow.AddDays(7)
            );

            try
            {
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error generating jwt token");
                return null;
            }           
        }
    }
}