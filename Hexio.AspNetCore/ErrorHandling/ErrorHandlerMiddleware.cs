using System;
using System.Threading.Tasks;
using Autofac;
using Hexio.AspNetCore.Logging.Extensions;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore.ErrorHandling
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ErrorHandlerMiddleware(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e) when (HandleError(e, context, out var result))
            {
                await context.WriteModel(result.StatusCode, result.Content);
            }
        }

        private bool HandleError(Exception e, HttpContext context, out ErrorResult errorResult)
        {
            errorResult = null;
           

            foreach (var exceptionType in e.GetType().GetBaseTypes())
            {
                if (!TryResolveHandler(exceptionType, out var handler))
                {
                    continue;
                }

                var result = handler.HandleError(e, context);

                if (result is ErrorNotHandledResult)
                {
                    return false;
                }

                context.SetOriginalError(e);

                errorResult = result;

                return true;
            }

            return false;
        }

        private bool TryResolveHandler(Type exceptionType, out IErrorHandler handler)
        {
            var errorHandlerType = typeof(IErrorHandler<>);
            var handlerType = errorHandlerType.MakeGenericType(exceptionType);

            handler = _lifetimeScope.ResolveOptional(handlerType) as IErrorHandler;

            return handler != null;
        }
    }
}
