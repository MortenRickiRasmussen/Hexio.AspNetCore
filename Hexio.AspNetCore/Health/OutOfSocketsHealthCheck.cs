using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hexio.AspNetCore.Health
{
    public class OutOfSocketsHealthCheck : IHealthCheck
    {
        public static bool OutOfSockets = true; 
        
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (OutOfSockets)
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("We are out of sockets"));
            }

            return Task.FromResult(
                HealthCheckResult.Healthy());
        }
    }
}