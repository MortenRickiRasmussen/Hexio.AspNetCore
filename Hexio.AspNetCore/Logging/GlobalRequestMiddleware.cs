using System.Threading.Tasks;
using Hexio.AspNetCore.Logging.Enrichers.Request;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Hexio.AspNetCore.Logging
{
    internal class GlobalRequestMiddleware : IMiddleware
    {
        private readonly HttpContextEnricher<IGlobalRequestEnricher> _httpContextEnricher;

        public GlobalRequestMiddleware(HttpContextEnricher<IGlobalRequestEnricher> httpContextEnricher)
        {
            _httpContextEnricher = httpContextEnricher;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (LogContext.Push(_httpContextEnricher.WithHttpContext(context)))
            {
                await next(context);
            }
        }
    }
}