using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Hexio.AspNetCore
{
    public static class HostExtensions
    {
        public static IHostBuilder ConfigureWebHostHexioDefaults<T>(this IHostBuilder host) where T : class
        {
            return host.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<T>()
                        .UseHexioDefaults();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }
    }
}