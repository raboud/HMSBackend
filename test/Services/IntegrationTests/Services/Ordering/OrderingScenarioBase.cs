using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.BuildingBlocks.IntegrationEventLogEF;
using Ordering.Infrastructure;
using Ordering.API;
using Ordering.API.Infrastructure;

namespace IntegrationTests.Services.Ordering
{
    public class OrderingScenarioBase
    {
        public TestServer CreateServer()
        {
			IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder();
            webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory() + "\\Services\\Ordering");
            webHostBuilder.UseStartup<OrderingTestsStartup>();
            webHostBuilder.ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddJsonFile("settings.json");
            });

			TestServer testServer = new TestServer(webHostBuilder);

            testServer.Host
                .MigrateDbContext<OrderingContext>((context, services) =>
                {
					IHostingEnvironment env = services.GetService<IHostingEnvironment>();
					IOptions<OrderingSettings> settings = services.GetService<IOptions<OrderingSettings>>();
					ILogger<OrderingContextSeed> logger = services.GetService<ILogger<OrderingContextSeed>>();

                    new OrderingContextSeed()
                        .SeedAsync(context, env, settings, logger)
                        .Wait();
                })
                .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            return testServer;
        }

        public static class Get
        {
            public static string Orders = "api/v1/orders";

            public static string OrderBy(int id)
            {
                return $"api/v1/orders/{id}";
            }
        }

        public static class Put
        {
            public static string CancelOrder = "api/v1/orders/cancel";
            public static string ShipOrder = "api/v1/orders/ship";
        }
    }
}
