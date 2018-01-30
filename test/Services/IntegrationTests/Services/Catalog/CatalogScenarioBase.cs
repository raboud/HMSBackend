﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.BuildingBlocks.IntegrationEventLogEF;
using HMS.Catalog.API;
using HMS.Catalog.API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace IntegrationTests.Services.Catalog
{
    public class CatalogScenarioBase
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
            private const int PageIndex = 0;
            private const int PageCount = 4;

            public static string Items(bool paginated = false)
            {
                return paginated 
                    ? "api/v1/catalog/items" + Paginated(PageIndex, PageCount)
                    : "api/v1/catalog/items";
            }

            public static string ItemById(int id)
            {
                return $"api/v1/catalog/items/{id}";
            }

            public static string ItemByName(string name, bool paginated = false)
            {
                return paginated
                    ? $"api/v1/catalog/items/withname/{name}" + Paginated(PageIndex, PageCount)
                    : $"api/v1/catalog/items/withname/{name}";
            }

            public static string Types = "api/v1/catalog/catalogtypes";

            public static string Brands = "api/v1/catalog/catalogbrands";

            public static string Filtered(int catalogTypeId, int catalogBrandId, bool paginated = false)
            {
                return paginated
                    ? $"api/v1/catalog/items/type/{catalogTypeId}/brand/{catalogBrandId}" + Paginated(PageIndex, PageCount)
                    : $"api/v1/catalog/items/type/{catalogTypeId}/brand/{catalogBrandId}";
            }

            private static string Paginated(int pageIndex, int pageCount)
            {
                return $"?pageIndex={pageIndex}&pageSize={pageCount}";
            }
        }
    }
}
