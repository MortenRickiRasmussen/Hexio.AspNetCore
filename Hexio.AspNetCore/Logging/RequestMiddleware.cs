using System;
using System.Threading.Tasks;
using Hexio.AspNetCore.Logging.Enrichers.Request;
using Hexio.AspNetCore.Logging.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging
{
internal class RequestMiddleware : IMiddleware
    {
        public const string MessageTemplate = "HTTP {Method} {RawUrl} responded {StatusCode} in {Elapsed} ms";
        private static readonly ILogger Logger = Serilog.Log.ForContext<RequestMiddleware>();

        private readonly HttpContextEnricher<IRequestEnricher> _httpContextEnricher;

        public RequestMiddleware(HttpContextEnricher<IRequestEnricher> httpContextEnricher)
        {
            _httpContextEnricher = httpContextEnricher;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.StartTimer();

            var level = LogEventLevel.Information;
            try
            {
                await next(context);

                var statusCode = context.Response.StatusCode;
                if (statusCode >= 500)
                {
                    level = LogEventLevel.Error;
                }

                Log(context, statusCode, level);
            }
            catch (Exception ex) when (LogError(context, ex))
            {
                // Never caught, because `LogError()` returns false.
            }
        }

        private void Log(HttpContext context, int statusCode, LogEventLevel level)
        {
            var exception = context.GetOriginalError()?.Exception;

            Log(context, statusCode, level, exception);
        }

        private bool LogError(HttpContext context, Exception ex)
        {
            Log(context, 500, LogEventLevel.Error, ex);

            return false;
        }

        private void Log(HttpContext context, int statusCode, LogEventLevel level, Exception exception)
        {
            var elapsed = context.GetElapsed();

            Logger
                .ForContext(_httpContextEnricher.WithHttpContext(context))
                .Write(level, exception, MessageTemplate, context.Request.Method, context.Request.GetFullUrl(), statusCode, elapsed);
        }
    }
}