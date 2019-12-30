using Microsoft.Extensions.DependencyInjection;

namespace Hexio.AspNetCore.Health
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHexioHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<OutOfSocketsHealthCheck>("OutOfSockets");

            return services;
        }
    }
}