using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Auth.WebApi.ApiBuilder.Other;

public static class SwaggerExtension
{
    public static SwaggerGenOptions Config(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            }
        );
        options.OperationFilter<SecurityRequirementsOperationFilter>();
        return options;
    }

    public static SwaggerGenOptions Config(
        this SwaggerGenOptions options,
        OpenApiSecurityScheme scheme
    )
    {
        options.AddSecurityDefinition("oauth2", scheme);
        options.OperationFilter<SecurityRequirementsOperationFilter>();
        return options;
    }
}
