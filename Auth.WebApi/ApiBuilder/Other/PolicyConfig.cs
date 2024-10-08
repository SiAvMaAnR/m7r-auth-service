using Auth.Domain.Shared.Constants.Common;
using Auth.Domain.Shared.Settings;
using Auth.Infrastructure.AppSettings;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Auth.WebApi.ApiBuilder.Other;

public static class PolicyConfigExtension
{
    private static readonly string[] s_allowOrigins = [];

    public static void CorsConfig(this CorsOptions corsOptions, IConfiguration configuration)
    {
        ClientSettings clientSettings = AppSettings.GetSection<ClientSettings>(configuration);

        string[] origins = [.. s_allowOrigins, clientSettings.BaseUrl];

        corsOptions.AddPolicy(
            CorsPolicyName.Default,
            policy =>
                policy.WithOrigins(origins).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
        );
    }
}
