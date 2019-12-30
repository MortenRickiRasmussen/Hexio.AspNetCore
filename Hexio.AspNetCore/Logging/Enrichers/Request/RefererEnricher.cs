using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class RefererEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!context.Request.Headers.TryGetValue("Referer", out var value))
            {
                return;
            }

            var actionProperty = new LogEventProperty("Referer", new ScalarValue(value));
            logEvent.AddPropertyIfAbsent(actionProperty);
        }
    }
}