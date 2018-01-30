using FunctionalTests.Middleware;
using Microsoft.AspNetCore.Builder;
using Marketing.API;
using Microsoft.Extensions.Configuration;

namespace FunctionalTests.Services.Marketing
{
    public class MarketingTestsStartup : Startup
	{
        public MarketingTestsStartup(IConfiguration configuration) : base(configuration)
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
