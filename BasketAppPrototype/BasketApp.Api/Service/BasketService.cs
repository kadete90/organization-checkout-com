using System;
using System.Linq;
using System.Threading.Tasks;
using BasketApp.Api.Data;
using BasketApp.Api.Data.Entities;
using BasketApp.Api.Models;
using BasketApp.Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasketApp.Api.Service
{
    public interface IBasketService
    {
        Task<BasketItemsModel> GetAsync(IdentityUser user);

        Task<bool?> AddOrUpdateItemAsync(Guid itemId, int amount, IdentityUser user);

        Task<bool?> RemoveItemAsync(Guid itemId, IdentityUser user);

        Task<bool> ClearAsync(IdentityUser user);
    }

    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;

        public BasketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BasketItemsModel> GetAsync(IdentityUser user)
        {            
            var userBasket = await _getBasketAsync(user.Id);

            return userBasket == null 
                ? new BasketItemsModel()
                : userBasket.ToModel();
        }

        public async Task<bool?> AddOrUpdateItemAsync(Guid itemId, int amount, IdentityUser user)
        {
            if (amount <= 0)
            {
                // alternative if amount == 0: remove the item from the basket
                throw new ArgumentException(nameof(amount));
            }

            if (!_context.Products.Any(p => p.Id == itemId))
            {
                return null;
            }

            var userBasket = await _getBasketAsync(user.Id);

            if (userBasket == null)
            {
                userBasket = new Basket
                {
                    Id = user.Id
                };

                await _context.Baskets.AddAsync(userBasket);
            }

            var basketItem = userBasket.BasketItems.SingleOrDefault(bi => bi.ProductId == itemId);
            if (basketItem != null)
            {
                basketItem.Amount = amount;
            }
            else
            {
                userBasket.BasketItems.Add(new BasketProducts
                {
                    ProductId = itemId,
                    BasketId = userBasket.Id,
                    Amount = amount
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // TODO : Add log
                return false;
            }

            return true;
        }

        public async Task<bool?> RemoveItemAsync(Guid itemId, IdentityUser user)
        {
            var userBasket = await _getBasketAsync(user.Id);

            var basketItem = userBasket?.BasketItems.SingleOrDefault(bi => bi.ProductId == itemId);

            if (basketItem == null)
            {
                return null;
            }

            _context.BasketProducts.Remove(basketItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // TODO : Add log
                return false;
            }

            return true;
        }

        public async Task<bool> ClearAsync(IdentityUser user)
        {
            var userBasket = await _getBasketAsync(user.Id);

            if (userBasket == null)
            {
                return true;
            }

            userBasket.BasketItems.Clear(); // TODO check if works

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // TODO : Add log
                return false;
            }

            return true;
        }

        private async Task<Basket> _getBasketAsync(string id)
        {
            return await _context.Baskets
                .Include(b => b.BasketItems)
                .SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}
