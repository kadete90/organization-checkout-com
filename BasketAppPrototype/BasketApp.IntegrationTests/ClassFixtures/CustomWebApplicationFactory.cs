using BasketApp.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BasketApp.IntegrationTests.ClassFixtures
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public CustomWebApplicationFactory()
        {

        }

        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.UseEnvironment("testing");
        //    base.ConfigureWebHost(builder);
        //}

        //public HttpClient HttpClient { get; }

        //public CustomWepApplicationFactory(WebApplicationFactory<Api.Startup> fixture)
        //{
        //    var factory = fixture.Factories.FirstOrDefault() ?? fixture.WithWebHostBuilder(ConfigureWebHostBuilder);
        //    var host = factory.Server?.Host;
        //    HttpClient = factory.CreateClient();

        //    _SeedData(host);
        //}

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("testing");
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                // TODO how to call this after the Api.Startup is fully configured to initializeData?

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    
                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        SeedData.InitializeAsync(scopedServices).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }
            });
        }
    }
}
