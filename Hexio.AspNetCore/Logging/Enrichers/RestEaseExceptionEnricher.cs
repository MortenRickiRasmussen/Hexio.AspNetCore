using System.Collections.Generic;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    public class RestEaseExceptionEnricher : ILogEventEnricher
    {
        private static readonly string[] ExtraFields = new[] { "Content", "ReasonPhrase", "StatusCode", "RequestUri" };

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception == null)
            {
                return;
            }

            var type = logEvent.Exception.GetType();

            if (type.FullName != "RestEase.ApiException")
            {
                return;
            }

            var properties = type.GetProperties().Where(x => ExtraFields.Contains(x.Name));

            var data = properties.ToDictionary(x => x.Name, x => x.GetValue(logEvent.Exception)) as IDictionary<string, object>;

            var property = propertyFactory.CreateProperty("RestError", data);

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}