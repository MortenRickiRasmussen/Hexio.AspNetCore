using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore.Logging.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetFullUrl(this HttpRequest request)
        {
            return request.Path + request.QueryString;
        }

        private const string ElapsedKey = "Elapsed";

        public static void StartTimer(this HttpContext context)
        {
            context.Items.Add(ElapsedKey, Stopwatch.GetTimestamp());
        }

        public static long GetElapsed(this HttpContext context)
        {
            if (context.Items.TryGetValue(ElapsedKey, out var obj) && obj is long elapsed)
            {
                var test = (Stopwatch.GetTimestamp() - elapsed) / ((double)Stopwatch.Frequency / 1000);

                return (long)test;
            }

            return 0;
        }

        private const string OriginalErrorKey = "OriginalError";

        internal static void SetOriginalError(this HttpContext context, Exception ex)
        {
            context.Items.Add(OriginalErrorKey, new OriginalError
            {
                Exception = ex,
            });
        }

        internal static OriginalError GetOriginalError(this HttpContext context)
        {
            if (context.Items.TryGetValue(OriginalErrorKey, out var obj) && obj is OriginalError original)
            {
                return original;
            }

            return null;
        }

        internal class OriginalError
        {
            public Exception Exception { get; set; }
        }
    }
}