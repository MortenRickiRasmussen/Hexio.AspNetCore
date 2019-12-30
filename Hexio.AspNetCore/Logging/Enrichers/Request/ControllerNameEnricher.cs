using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class ControllerNameEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var routeData = context.GetRouteData();

            if (routeData == null)
            {
                return;
            }

            if (!routeData.Values.TryGetValue("controller", out var controllerObj))
            {
                return;
            }

            var name = controllerObj as string;

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var controllerProperty = new LogEventProperty("ControllerName", new ScalarValue(name));
            logEvent.AddPropertyIfAbsent(controllerProperty);
        }
    }
}