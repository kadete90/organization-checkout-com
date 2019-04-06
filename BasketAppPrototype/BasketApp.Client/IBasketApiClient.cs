using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketApp.Common.Contracts;

namespace BasketApp.Client
{
    interface IBasketApiClient
    {
        Task<bool> AuthenticateAsync(string username, string password);

        Task<IEnumerable<ProductModel>> GetProductsAsync();

        Task<BasketModel> GetUserBasket();
        Task AddItemToBasket(ProductUpdateModel item);
        Task UpdateItemAmountInBasket(ProductUpdateModel item);
        Task RemoveItemFromBasketAsync(Guid itemId);
        Task ClearBasketAsync();
    }
}
