using System;
using System.Threading.Tasks;
using BasketApi.Common;
using BasketApp.Client;
using Xunit;

namespace BasketApp.IntegrationTests
{
    public class AuthTests
    {
        public static readonly Uri BaseHttpsUrl = new Uri("https://localhost:44341");

        [Fact]
        public async Task ValidCredentials()
        {
            BasketApiClient client = new BasketApiClient(BaseHttpsUrl);
            Assert.True(await client.AuthenticateAsync(TestConstants.TesterUserName, "testPw1!"));
        }

        [Fact]
        public async Task InvalidCredentialsAsync()
        {
            BasketApiClient client = new BasketApiClient(BaseHttpsUrl);
            await Assert.ThrowsAsync<UnauthorizedAccessException>( () => client.AuthenticateAsync("test", "testPw1!") );
        }
    }
}
