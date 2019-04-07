using BasketApp.Common;
using BasketApp.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using BasketApp.Common.Contracts;
using System.Linq;

namespace BasketApp.IntegrationTests
{
    public class BasketTests
    {
        public static readonly Uri BaseHttpsUrl = new Uri(TestConstants.BaseApiAddress);
        readonly BasketApiClient apiClient;

        public BasketTests()
        {
            apiClient = new BasketApiClient(BaseHttpsUrl);
        }

        [Fact]
        public async Task Basket_Operations_Test()
        {
            Assert.True(await apiClient.AuthenticateAsync(TestConstants.TesterUserName, TestConstants.TesterPassword));

            await apiClient.ClearBasketAsync();

            var basket = await apiClient.GetUserBasket();
            Assert.Empty(basket.Items);

            var products = (await apiClient.GetProductsAsync()).ToArray();

            Assert.True(products.Length > 2);

            var p1 = new ProductUpdateModel
            {
                Id = products[0].Id,
                Amount = 5
            };
            var p2 = new ProductUpdateModel
            {
                Id = products[1].Id,
                Amount = 2
            };
            var p3 = new ProductUpdateModel
            {
                Id = products[2].Id,
                Amount = 8
            };

            await apiClient.AddItemToBasket(p1);
            await apiClient.AddItemToBasket(p2);
            await apiClient.AddItemToBasket(p3);

            basket = await apiClient.GetUserBasket();
            Assert.Equal(3, basket.Items.Count);

            await apiClient.RemoveItemFromBasketAsync(p1.Id);

            basket = await apiClient.GetUserBasket();
            Assert.Equal(2, basket.Items.Count);

            p2.Amount = 5;
            await apiClient.UpdateItemAmountInBasket(p2);
            basket = await apiClient.GetUserBasket(); // TODO add get basket/items/{id} endpoint
            Assert.Equal(p2.Amount, basket.Items.SingleOrDefault(i => i.Id == p2.Id).Amount);

            await apiClient.ClearBasketAsync();
            basket = await apiClient.GetUserBasket();
            Assert.Empty(basket.Items);
        }
    }
}
