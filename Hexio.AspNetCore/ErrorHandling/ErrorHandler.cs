using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore.ErrorHandling
{
    public interface IErrorHandler
    {
        ErrorResult HandleError(Exception e, HttpContext context);
    }

    public interface IErrorHandler<TException> : IErrorHandler where TException : Exception
    {
        ErrorResult Handle(TException e, HttpContext context);
    }

    public abstract class ErrorHandler<TException> : IErrorHandler<TException>
        where TException : Exception
    {
        public abstract ErrorResult Handle(TException e, HttpContext context);

        public ErrorResult HandleError(Exception e, HttpContext context)
        {
            return Handle(e as TException, context);
        }

        public ErrorResult Ok()
        {
            return StatusCode(HttpStatusCode.OK);
        }

        public ErrorResult BadRequest(object content)
        {
            return StatusCode(HttpStatusCode.BadRequest, content);
        }

        public ErrorResult StatusCode(int statusCode, object content = null)
        {
            return new ErrorResult
            {
                StatusCode = statusCode,
                Content = content,
            };
        }

        public ErrorResult StatusCode(HttpStatusCode statusCode, object content = null)
        {
            return StatusCode((int)statusCode, content);
        }

        public ErrorResult ExceptionNotHandled()
        {
            return new ErrorNotHandledResult();
        }
    }
}
