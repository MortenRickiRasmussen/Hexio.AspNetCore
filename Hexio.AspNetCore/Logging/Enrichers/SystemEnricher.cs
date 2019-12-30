using System;
using System.Runtime;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    public class SystemEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var property = propertyFactory.CreateProperty("System", new
            {
                Environment.ProcessorCount,
                OSVersion = Environment.OSVersion.ToString(),
                GCSettings.IsServerGC,
            }, true);

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}