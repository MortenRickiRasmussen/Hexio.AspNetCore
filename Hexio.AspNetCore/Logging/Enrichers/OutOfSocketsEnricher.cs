using System.Net.Sockets;
using Hexio.AspNetCore.Health;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    internal class OutOfSocketsEnricher : ILogEventEnricher
    {
        //https://docs.microsoft.com/en-us/windows/desktop/winsock/windows-sockets-error-codes-2

        private const string ExceptionMessage = "Address in use";
        private const int ErrorCode = 10048;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!(logEvent.Exception is System.Net.Http.HttpRequestException requestException))
            {
                return;
            }

            if (!(requestException?.InnerException is SocketException socketException))
            {
                return;
            }

            if (socketException.Message == ExceptionMessage || socketException.ErrorCode == ErrorCode)
            {
                Log.Fatal("Service ran out of sockets, restarting service");

                OutOfSocketsHealthCheck.OutOfSockets = true;
            }
        }
    }
}