using System.Threading.RateLimiting;
using Auth.Domain.Shared.Constants.Common;
using Microsoft.AspNetCore.RateLimiting;

namespace Auth.WebApi.ApiBuilder.Configurations;

public static class RateLimiterConfigExtension
{
    public static void RateLimiterConfig(this RateLimiterOptions rateLimiterOptions)
    {
        rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        rateLimiterOptions.AddPolicy(
            RateLimiterPolicyName.Auth,
            httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions()
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    }
                )
        );
    }
}
