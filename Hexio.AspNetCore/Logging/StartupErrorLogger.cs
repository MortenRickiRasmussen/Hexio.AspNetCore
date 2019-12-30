using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Hexio.AspNetCore.Logging
{
    public class StartupErrorLogger : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                try
                {
                    next(builder);
                }
                catch (Exception e) when (LogException(e))
                {
                    throw;
                }
            };
        }

        private bool LogException(Exception e)
        {
            Log.ForContext<StartupErrorLogger>().Fatal(e, "Service startup failure");

            Log.CloseAndFlush();

            return false;
        }
    }
}