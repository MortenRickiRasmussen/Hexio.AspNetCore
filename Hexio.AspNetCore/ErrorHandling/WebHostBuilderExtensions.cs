using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hexio.AspNetCore.ErrorHandling
{
    internal static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder AddDomainExceptionHandler(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddScoped<ErrorHandlerMiddleware>();
                services.AddScoped<IErrorHandler<DomainException>, DomainExceptionHandler>();
                services.AddScoped<IErrorHandler<ConnectionResetException>, ConnectionResetHandler>();

                services.AddSingleton<IStartupFilter>(_ => new StartupFilter(builder =>
                {
                    builder.UseMiddleware<ErrorHandlerMiddleware>();
                }));
            });

            return hostBuilder;
        }
    }
}
