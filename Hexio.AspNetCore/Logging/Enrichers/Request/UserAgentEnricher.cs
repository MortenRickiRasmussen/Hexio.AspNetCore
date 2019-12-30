using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class UserAgentEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!context.Request.Headers.TryGetValue("User-Agent", out var value))
            {
                return;
            }

            var actionProperty = new LogEventProperty("UserAgent", new ScalarValue(value));
            logEvent.AddPropertyIfAbsent(actionProperty);
        }
    }
}