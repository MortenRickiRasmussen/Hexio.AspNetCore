using System;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore.ErrorHandling
{
    internal class DomainExceptionHandler : ErrorHandler<DomainException>
    {
        public override ErrorResult Handle(DomainException e, HttpContext context)
        {
            return BadRequest(new
            {
                message = e.Message,
            });
        }
    }

    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
