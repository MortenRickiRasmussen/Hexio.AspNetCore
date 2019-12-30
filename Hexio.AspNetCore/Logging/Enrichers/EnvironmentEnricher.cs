using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    public class EnvironmentEnricher : ILogEventEnricher
    {
        private readonly string _environmentName;

        public EnvironmentEnricher(string environmentName)
        {
            _environmentName = environmentName;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var property = new LogEventProperty("Environment", new ScalarValue(_environmentName));

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}