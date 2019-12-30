using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    internal class RequestBodyEnricher : IRequestEnricher
    {
        private static readonly IList _allowedContentTypes = new List<string> { "application/json", "x-www-form-urlencoded", "form-data" };

        public void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (context.Response.StatusCode < 400)
            {
                return;
            }

            if (!_allowedContentTypes.Contains(context.Request.ContentType))
            {
                return;
            }

            var bodyStream = new StreamReader(context.Request.Body);
            var bodyText = bodyStream.ReadToEnd();

            bodyText = RequestBodyFilter.Get(bodyText);

            var bodyProperty = new LogEventProperty("Body", new ScalarValue(bodyText));
            logEvent.AddPropertyIfAbsent(bodyProperty);
        }
    }
}