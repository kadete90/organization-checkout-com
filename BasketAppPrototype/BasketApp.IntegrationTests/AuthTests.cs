using System.Net.Http;
using System.Threading.Tasks;
using BasketApp.Client;
using BasketApp.Common;
using System;
using Xunit;

namespace BasketApp.IntegrationTests
{
    public class AuthTests
        //: IClassFixture<CustomWebApplicationFactory<BasketApp.Api.Startup>>
    {
        public static readonly Uri BaseHttpsUrl = new Uri(TestConstants.BaseApiAddress);
        //private readonly HttpClient _client;
        //private readonly CustomWebApplicationFactory<BasketApp.Api.Startup> _factory;

        public AuthTests()
        {
            // TODO : find the proper way of doing this
            //var projectDir = Path.Combine(Path.GetFullPath("../../../../BasketApp.Api"));

            //var server = new TestServer(
            //    new WebHostBuilder()
            //            .UseEnvironment("Development")
            //            .UseConfiguration(new ConfigurationBuilder()
            //                .SetBasePath(projectDir)
            //                .AddJsonFile("appsettings.json")
            //                .AddEnvironmentVariables()
            //                .Build()
            //            )
            //            .UseStartup<Api.Startup>()
            //);
            //server.BaseAddress = BaseHttpsUrl;
            //_client = server.CreateClient();
        }

        //public AuthTests(CustomWebApplicationFactory<BasketApp.Api.Startup> factory)
        //{
        //    _factory = factory;
        //    _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        //    {
        //        AllowAutoRedirect = false,
        //        BaseAddress = BaseHttpsUrl
        //    });
        //}

        [Fact]
        public async Task ValidCredentials()
        {
            BasketApiClient apiClient = new BasketApiClient(BaseHttpsUrl);
            //BasketApiClient apiClient = new BasketApiClient(_client);

            var result = await apiClient.AuthenticateAsync(TestConstants.TesterUserName, TestConstants.TesterPassword);

            Assert.True(result);
        }

        [Fact]
        public async Task InvalidCredentialsAsync()
        {
            BasketApiClient apiClient = new BasketApiClient(BaseHttpsUrl);
            //BasketApiClient apiClient = new BasketApiClient(_client);

            // TODO : proper validation on status code unauthorized
            await Assert.ThrowsAsync<HttpRequestException>(() => apiClient.AuthenticateAsync(TestConstants.TesterUserName, "invalidPW"));
        }
    }
}
