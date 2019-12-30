using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class RouteValuesEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var routeData = context.GetRouteData();

            if (routeData == null)
            {
                return;
            }

            var valuePairs = routeData.Values
                .Where(x => x.Key != "controller")
                .Where(x => x.Key != "action")
                .ToDictionary(x => x.Key, x => x.Value) as IReadOnlyDictionary<string, object>;

            if (!valuePairs.Any())
            {
                return;
            }

            var property = propertyFactory.CreateProperty("RouteData", valuePairs);

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}