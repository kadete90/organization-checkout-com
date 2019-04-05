using System.Threading.Tasks;
using BasketApi.Common.Contracts;
using BasketApp.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]

        public async Task<IActionResult> GetToken([FromBody] CredentialsModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _userService.GetTokenAsync(model.Username, model.Password);

                if (token == null)
                {
                    return BadRequest("Invalid Username or password");
                }

                return Ok(new {
                    authenticated = true,
                    token = token
                });
            }
            return BadRequest(ModelState);
        }
    }
}
