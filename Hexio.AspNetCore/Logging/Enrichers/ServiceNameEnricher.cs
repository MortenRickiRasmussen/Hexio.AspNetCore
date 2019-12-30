using System;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    public class ServiceNameEnricher : ILogEventEnricher
    {
        private readonly Lazy<string> _serviceName = new Lazy<string>(GetServiceName);

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var property = new LogEventProperty("ServiceName", new ScalarValue(_serviceName.Value));

            logEvent.AddPropertyIfAbsent(property);
        }

        public static string GetServiceName()
        {
            var assembly = Assembly.GetEntryAssembly();
                
            if (assembly == null)
            {
                return "TestRunner";
            }

            return assembly.GetName().Name;
        }
    }
}