using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers.Request
{
    public interface IRequestEnricher
    {
        void Enrich(HttpContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory);
    }

    public interface IGlobalRequestEnricher : IRequestEnricher
    {
    }

    internal class HttpContextEnricher<T> where T : IRequestEnricher
    {
        private readonly IEnumerable<T> _enrichers;

        public HttpContextEnricher(IEnumerable<T> enrichers)
        {
            _enrichers = enrichers;
        }

        public RequestEnricher<T> WithHttpContext(HttpContext context)
        {
            return new RequestEnricher<T>(_enrichers, context);
        }
    }

    internal class RequestEnricher<T> : ILogEventEnricher where T : IRequestEnricher
    {
        private readonly IEnumerable<T> _enrichers;
        private readonly HttpContext _httpContext;

        public RequestEnricher(IEnumerable<T> enrichers, HttpContext httpContext)
        {
            _enrichers = enrichers;
            _httpContext = httpContext;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            foreach (var enricher in _enrichers)
            {
                enricher.Enrich(_httpContext, logEvent, propertyFactory);
            }
        }
    }
}