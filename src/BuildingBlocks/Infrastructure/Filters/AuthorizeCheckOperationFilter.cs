using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
			var found = false;
			if (context.ApiDescription.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller)
			{
				found = controller.ControllerTypeInfo.GetCustomAttributes().OfType<AuthorizeAttribute>().Any();
			}
			if (!found)
			{
				if (!context.ApiDescription.TryGetMethodInfo(out var mi)) return;
				if (!mi.GetCustomAttributes().OfType<AuthorizeAttribute>().Any()) return;
			}

			operation.Responses.TryAdd("401", new Response { Description = "Unauthorized" });
			operation.Responses.TryAdd("403", new Response { Description = "Forbidden" });

			operation.Security = new List<IDictionary<string, IEnumerable<string>>>
			{
				new Dictionary<string, IEnumerable<string>>
				{
					{ "oauth2", new [] { "identityapi" } }
				}
			};

		}
	}
}