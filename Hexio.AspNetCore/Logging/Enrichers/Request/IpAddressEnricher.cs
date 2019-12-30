using Hexio.AspNetCore.Logging.Enrichers.Request;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    internal class IpAddressEnricher : IRequestEnricher
    {
        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var remoteAddress = context.Connection.RemoteIpAddress;

            if (remoteAddress == null)
            {
                return;
            }

            var ipAddress = remoteAddress.ToString();

            if (ipAddress == "::1")
            {
                return;
            }

            if (context.Request.Headers.TryGetValue("X-FORWARDED-FOR", out var value))
            {
                ipAddress = value;
            }

            var property = new LogEventProperty("HttpClientIp", new ScalarValue(ipAddress));

            logEvent.AddPropertyIfAbsent(property);
        }
    }
}