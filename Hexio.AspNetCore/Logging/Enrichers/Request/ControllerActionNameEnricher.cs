using Hexio.AspNetCore.Logging.Enrichers.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    internal class ControllerActionNameEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var routeData = context.GetRouteData();

            if (routeData == null)
            {
                return;
            }

            if (!routeData.Values.TryGetValue("action", out var actionObj))
            {
                return;
            }

            var name = actionObj as string;

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var actionProperty = new LogEventProperty("ControllerAction", new ScalarValue(name));
            logEvent.AddPropertyIfAbsent(actionProperty);
        }
    }
}