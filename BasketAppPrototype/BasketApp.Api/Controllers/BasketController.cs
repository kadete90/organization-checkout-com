using System;
using System.Threading.Tasks;
using BasketApi.Common.Contracts;
using BasketApp.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BasketApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly UserManager<IdentityUser> _userManager;

        public BasketController(IBasketService basketService, UserManager<IdentityUser> userManager)
        {
            _basketService = basketService;
            _userManager = userManager;
        }

        // GET api/basket
        [HttpGet]
        [ProducesResponseType(typeof(BasketItemsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            return Ok(await _basketService.GetAsync(currentUser));
        }

        // POST api/basket/items
        [HttpPost("items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItemToBasket([FromBody] ProductAmountModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var result = await _basketService.AddOrUpdateItemAsync(model.ProductId, model.Amount, currentUser);
            if (result == null)
            {
                return NotFound("This item doesn't exist");
            }

            return Ok("Item updated with success!");
        }

        // PUT api/basket/items/guid
        [HttpPut("items/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItemAmountInBasket(Guid id, [FromBody] ProductUpdateAmountModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var result = await _basketService.AddOrUpdateItemAsync(id, model.Amount, currentUser);
            if (result == null)
            {
                return NotFound("This item doesn't exist");
            }

            return Ok("Item updated with success!");
        }

        // DELETE api/basket/items/guid
        [HttpDelete("items/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItemFromBasket(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var result = await _basketService.RemoveItemAsync(id, currentUser);
            if (result == null)
            {
                return NotFound("This item doesn't exist in the basket");
            }

            return Ok("Item removed with success!");
        }

        // DELETE api/basket/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearBasket()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            if (await _basketService.ClearAsync(currentUser))
            {
                return Ok("User Basket cleared");
            }

            return BadRequest();
        }
    }
}
