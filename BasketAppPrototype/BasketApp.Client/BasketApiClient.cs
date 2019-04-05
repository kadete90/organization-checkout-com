using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BasketApi.Common.Contracts;
using Newtonsoft.Json;

namespace BasketApp.Client
{
    public class BasketApiClient : IBasketApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        private const string TokenBaseUrl = "/api/token";
        private const string BasketBaseUrl = "/api/basket";
        private const string BasketItemsBaseUrl = "/api/basket/items";

        public BasketApiClient(Uri baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = baseAddress };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var uri = new Uri(TokenBaseUrl, UriKind.RelativeOrAbsolute);

            var credentialsModel = new CredentialsModel
            {
                Username = username,
                Password = password
            }.AsJson();

            var response = await _httpClient.PostAsync(uri, credentialsModel);

            var rawToken = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rawToken);
            return true;
        }

        public async Task<BasketItemsModel> GetUserBasket()
        {
            var uri = new Uri(BasketBaseUrl, UriKind.Relative);
            var response = await _httpClient.GetAsync(uri);
            var responseAsString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasketItemsModel>(responseAsString);
        }

        public async Task AddItemToBasket(string basketId, ProductModel item)
        {
            var uri = new Uri(BasketItemsBaseUrl, UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, item.AsJson());
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateItemAmountInBasket(string basketId, ProductAmountModel item)
        {
            var uri = new Uri(BasketItemsBaseUrl, UriKind.Relative);
            var response = await _httpClient.PutAsync(uri, item.AsJson());
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveItemFromBasketAsync(Guid itemId)
        {
            var response = await _httpClient.DeleteAsync($"{BasketBaseUrl}/{itemId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearBasketAsync(string basketId)
        {
            var response = await _httpClient.DeleteAsync($"{BasketBaseUrl}/clear");
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
