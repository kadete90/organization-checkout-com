using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BasketApp.Common.Contracts;
using BasketApp.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BasketApp.Client
{
    public class BasketApiClient : IBasketApiClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        private const string TokenBaseUrl = "/api/account/token";
        private const string BasketBaseUrl = "/api/basket";
        private const string BasketItemsBaseUrl = "/api/basket/items";
        private const string ProductsBaseUrl = "/api/products";

        public BasketApiClient(Uri baseAddress)
        {
            _httpClient = new HttpClient { BaseAddress = baseAddress };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public BasketApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

            var responseAsString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TokenModel>(responseAsString);
            if (!model.Authenticated)
                return false;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);
            return true;
        }

        public async Task<BasketModel> GetUserBasket()
        {
            var uri = new Uri(BasketBaseUrl, UriKind.Relative);
            var response = await _httpClient.GetAsync(uri);
            var responseAsString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BasketModel>(responseAsString);
        }

        public async Task AddItemToBasket(ProductUpdateModel item)
        {
            var uri = new Uri(BasketItemsBaseUrl, UriKind.Relative);
            var response = await _httpClient.PostAsync(uri, item.AsJson());
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateItemAmountInBasket(ProductUpdateModel item)
        {
            var uri = new Uri($"{BasketItemsBaseUrl}/{item.Id}", UriKind.Relative);
            var response = await _httpClient.PutAsync(uri, item.AsJson());
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveItemFromBasketAsync(Guid itemId)
        {
            var uri = new Uri($"{BasketItemsBaseUrl}/{itemId}", UriKind.Relative);
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearBasketAsync()
        {
            var response = await _httpClient.DeleteAsync($"{BasketBaseUrl}/clear");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            var uri = new Uri(ProductsBaseUrl, UriKind.Relative);
            var response = await _httpClient.GetAsync(uri);
            var responseAsString = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductModel>>(responseAsString);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
