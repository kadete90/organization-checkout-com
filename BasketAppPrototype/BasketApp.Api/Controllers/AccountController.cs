using System.Threading.Tasks;
using BasketApp.Api.Models;
using BasketApp.Api.Service;
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
        [Route("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var token = await _userService.GetTokenAsync(loginModel.Username, loginModel.Password);

                if (token == null)
                {
                    return BadRequest("Invalid Username or password");
                }

                return Ok(token);
            }
            return BadRequest(ModelState);
        }
    }
}
