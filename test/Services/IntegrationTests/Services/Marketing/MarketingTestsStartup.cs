﻿using Marketing.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using IntegrationTests.Middleware;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Services.Marketing
{
    public class MarketingTestsStartup : Startup
    {
        public MarketingTestsStartup(IConfiguration env) : base(env)
        {
        }

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                app.UseMiddleware<AutoAuthorizeMiddleware>();
            }
            else
            {
                base.ConfigureAuth(app);
            }
        }
    }
}
