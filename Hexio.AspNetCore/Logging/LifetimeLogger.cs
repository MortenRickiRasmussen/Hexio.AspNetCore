using System;
using System.Diagnostics;
using Hexio.AspNetCore.Logging.Enrichers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Core;

namespace Hexio.AspNetCore.Logging
{
    public class LifetimeLogger : IStartupFilter
    {
        private static readonly ILogEventEnricher[] _lifetimeEnrichers = new ILogEventEnricher[]
        {
            new SystemEnricher(),
        };

        private Stopwatch _startTime;
        private Stopwatch _stopTime;

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Starting();

            return builder =>
            {
                var lifetime = builder.ApplicationServices.GetService(typeof(IApplicationLifetime)) as IApplicationLifetime;

                lifetime?.ApplicationStarted.Register(Started);
                lifetime?.ApplicationStopping.Register(Stopping);
                lifetime?.ApplicationStopped.Register(Stopped);

                next(builder);
            };
        }

        private void Starting()
        {
            _startTime = Stopwatch.StartNew();

            Log.ForContext<LifetimeLogger>()
                .ForContext(_lifetimeEnrichers)
                .Information("Service starting");
        }

        private void Started()
        {
            _startTime.Stop();
            var elapsed = _startTime.ElapsedMilliseconds;

            Log.ForContext<LifetimeLogger>().Information("Service started after {Elapsed} ms", elapsed);
        }

        private void Stopping()
        {
            _stopTime = Stopwatch.StartNew();

            Log.ForContext<LifetimeLogger>().Information("Service stopping");
        }

        private void Stopped()
        {
            _stopTime.Stop();
            var elapsed = _stopTime.ElapsedMilliseconds;

            Log.ForContext<LifetimeLogger>().Information("Service stopped after {Elapsed} ms", elapsed);
        }
    }
}