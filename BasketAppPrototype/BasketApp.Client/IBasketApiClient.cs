using System;
using System.Net.Http;
using System.Threading.Tasks;
using BasketApi.Common.Contracts;

namespace BasketApp.Client
{
    interface IBasketApiClient
    {
        Task<bool> AuthenticateAsync(string username, string password);

        Task<BasketItemsModel> GetUserBasket();
        Task AddItemToBasket(string basketId, ProductModel item);
        Task UpdateItemAmountInBasket(string basketId, ProductAmountModel item);
        Task RemoveItemFromBasketAsync(Guid itemId);
        Task ClearBasketAsync(string basketId);
    }
}
