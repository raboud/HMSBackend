using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using HMS.Catalog.API;
using HMS.Catalog.API.Infrastructure;

namespace FunctionalTests.Services.Catalog
{
    public class CatalogScenariosBase
    {
        public TestServer CreateServer()
        {
            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder();
            webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory() + "\\Services\\Catalog");
            webHostBuilder.UseStartup<Startup>();

			TestServer testServer = new TestServer(webHostBuilder);

            testServer.Host
                .MigrateDbContext<CatalogContext>((context, services) =>
                {
					IHostingEnvironment env = services.GetService<IHostingEnvironment>();
					IOptions<CatalogSettings> settings = services.GetService<IOptions<CatalogSettings>>();
					ILogger<CatalogContextSeed> logger = services.GetService<ILogger<CatalogContextSeed>>();

                    new CatalogContextSeed()
                    .SeedAsync(context, env, settings, logger)
                    .Wait();
                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            return testServer;
        }

        public static class Get
        {
            public static string Orders = "api/v1/orders";

            public static string Items = "api/v1/catalog/items";

            public static string ProductByName(string name)
            {
                return $"api/v1/catalog/items/withname/{name}";
            }
        }

        public static class Put
        {
            public static string UpdateCatalogProduct = "api/v1/catalog/items";
        }
    }
}
