using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Hexio.AspNetCore
{
    internal static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public static async Task WriteModel(this HttpContext context, int statusCode, object model = null)
        {
            if (model == null)
            {
                context.Response.StatusCode = statusCode;

                return;
            }

            var result = new ObjectResult(model)
            {
                StatusCode = statusCode,
                DeclaredType = model.GetType(),
            };

            await context.ExecuteResult(result);
        }

        public static async Task ExecuteResult<TResult>(this HttpContext context, TResult result)
            where TResult : IActionResult
        {
            var executor = context.RequestServices.GetRequiredService<IActionResultExecutor<TResult>>();

            var routeData = context.GetRouteData() ?? EmptyRouteData;
            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            await executor.ExecuteAsync(actionContext, result);
        }
    }
}