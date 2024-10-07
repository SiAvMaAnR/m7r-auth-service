using Auth.Domain.Shared.Constants.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Auth.WebApi.ApiBuilder.Other;

public static class PolicyConfigExtension
{
    private static readonly string[] s_allowOrigins =
    [
        "http://localhost:3000",
        "https://localhost:3000",
    ];

    public static void CorsConfig(this CorsOptions corsOptions)
    {
        corsOptions.AddPolicy(
            CorsPolicyName.Default,
            policy =>
                policy
                    .WithOrigins(s_allowOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
        );
    }
}
