﻿using System.Threading.Tasks;
using BasketApp.Common.Contracts;
using BasketApp.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BasketApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _userService;
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IAccountService userService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> GetToken([FromBody] CredentialsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (!loginResult.Succeeded)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized();

            var token = _userService.GenerateToken(user);
            if (token == null)
            {
                return Unauthorized(); // send internalServerError
            }

            return Ok(new TokenModel
            {
                Authenticated = true,
                Token = token
            });
        }
    }
}
