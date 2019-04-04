using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BasketApp.Api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BasketApp.Api.Service
{
    public interface IUserService
    {
        Task<string> GetTokenAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetTokenAsync(string username, string password)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (!loginResult.Succeeded)
                return null;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

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
                Console.WriteLine(e);
                throw;
            }           
        }
    }
}