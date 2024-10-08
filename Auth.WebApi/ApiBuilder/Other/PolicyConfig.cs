using Auth.Domain.Shared.Constants.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Auth.WebApi.ApiBuilder.Other;

public static class PolicyConfigExtension
{
    private static readonly string[] s_allowOrigins = [];

    public static void CorsConfig(this CorsOptions corsOptions)
    {
        string allowedOriginUrl = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN_URL") ?? "";

        corsOptions.AddPolicy(
            CorsPolicyName.Default,
            policy =>
                policy
                    .WithOrigins([.. s_allowOrigins, allowedOriginUrl])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
        );
    }
}
