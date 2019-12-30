using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddRootEndpoint(this IApplicationBuilder builder)
        {
            var serviceName = Assembly.GetCallingAssembly().GetName().Name;

            return builder.MapWhen(context => context.Request.Path == "/", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"Welcome to {serviceName}");
                });
            });
        }
    }
}