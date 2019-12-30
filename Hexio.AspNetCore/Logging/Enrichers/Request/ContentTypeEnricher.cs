using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class ContentTypeEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!context.Request.Headers.TryGetValue("Content-Type", out var value))
            {
                value = "None";
            }

            var actionProperty = new LogEventProperty("ContentType", new ScalarValue(value));
            logEvent.AddPropertyIfAbsent(actionProperty);
        }
    }
}