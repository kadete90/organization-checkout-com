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
        //: IClassFixture<CustomWebApplicationFactory<BasketApp.Api.Startup>>
    {
        public static readonly Uri BaseHttpsUrl = new Uri(TestConstants.BaseApiAddress);

        BasketApiClient apiClient;

        [Fact]
        public async Task Basket_Operations_Test()
        {
            apiClient = new BasketApiClient(BaseHttpsUrl);
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

        //[Fact]
        //public async Task Get_User_Basket_Test()
        //{
        //    var basket = await apiClient.GetUserBasket();

        //    //Assert.AreEqual(1, basket.Items.Count);
        //}

        //[Fact]
        //public async Task Add_Item_to_Basket_Test()
        //{

        //}

        //[Fact]
        //public async Task Update_Items_on_Basket_Test()
        //{

        //}

        //[Fact]
        //public async Task Delete_Item_of_Basket_Test()
        //{

        //}

        //[Fact]
        //public async Task Clear_Basket_Test()
        //{

        //}
    }
}
