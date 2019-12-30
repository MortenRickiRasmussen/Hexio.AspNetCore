using Hexio.AspNetCore.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Hexio.AspNetCore
{
    public static class WebHostbuilderExtensions
    {
        /// <summary>
        /// Removes server header.
        /// Adds logging.
        /// </summary>
        public static IWebHostBuilder UseHexioDefaults(this IWebHostBuilder builder)
        {
            builder.ConfigureLogging();
            
            builder.AddRequestLogging();

            return builder;
        }
    }
}