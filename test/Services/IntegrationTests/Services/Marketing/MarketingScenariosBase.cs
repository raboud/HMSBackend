using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Marketing.API.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace IntegrationTests.Services.Marketing
{
    public class MarketingScenarioBase
    {
        public static string CampaignsUrlBase => "api/v1/campaigns";

        public TestServer CreateServer()
        {
			IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder();
            webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory() + "\\Services\\Marketing");
            webHostBuilder.UseStartup<MarketingTestsStartup>();

			TestServer testServer = new TestServer(webHostBuilder);

            testServer.Host
               .MigrateDbContext<MarketingContext>((context, services) =>
               {
				   ILogger<MarketingContextSeed> logger = services.GetService<ILogger<MarketingContextSeed>>();

                   new MarketingContextSeed()
                       .SeedAsync(context, logger)
                       .Wait();

               });

            return testServer;
        }
    }
}