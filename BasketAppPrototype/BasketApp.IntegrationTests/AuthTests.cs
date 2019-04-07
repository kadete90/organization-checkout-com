using System.Net.Http;
using System.Threading.Tasks;
using BasketApp.Client;
using BasketApp.Common;
using Xunit;
using System;

namespace BasketApp.IntegrationTests
{
    public class AuthTests 
        //: IClassFixture<CustomWebApplicationFactory<Api.Startup>>
    {
        //private readonly CustomWebApplicationFactory<Api.Startup> _factory;
        //public AuthTests(CustomWebApplicationFactory<Api.Startup> factory)
        //{
        //    _factory = factory;
        //}

        public static readonly Uri BaseHttpsUrl = new Uri(TestConstants.BaseApiAddress);
        readonly BasketApiClient apiClient;

        public AuthTests()
        {
            apiClient = new BasketApiClient(BaseHttpsUrl);
        }

        [Fact]
        public async Task ValidCredentials()
        {
            //BasketApiClient apiClient = new BasketApiClient(_factory.CreateClient());

            var result = await apiClient.AuthenticateAsync(TestConstants.TesterUserName, TestConstants.TesterPassword);

            Assert.True(result);
        }

        [Fact]
        public async Task InvalidCredentialsAsync()
        {
            //BasketApiClient apiClient = new BasketApiClient(_factory.CreateClient());

            // TODO : proper validation on status code unauthorized
            await Assert.ThrowsAsync<HttpRequestException>(() => apiClient.AuthenticateAsync(TestConstants.TesterUserName, "invalidPW"));
        }
    }
}
