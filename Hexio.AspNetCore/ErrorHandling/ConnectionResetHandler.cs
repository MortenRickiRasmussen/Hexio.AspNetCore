using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;

namespace Hexio.AspNetCore.ErrorHandling
{
    internal class ConnectionResetHandler : ErrorHandler<ConnectionResetException>
    {
        public override ErrorResult Handle(ConnectionResetException e, HttpContext context)
        {
            return StatusCode(499, null);
        }
    }
}
