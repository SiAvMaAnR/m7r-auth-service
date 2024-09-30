using Auth.WebApi.Common;

namespace Auth.WebApi.ApiBuilder.ApplicationBuilder;

public static partial class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddEnvironmentConfiguration(
        this WebApplication webApplication
    )
    {
        return webApplication.Environment.EnvironmentName switch
        {
            AppEnvironment.Production => webApplication.ProductionConfiguration(),
            AppEnvironment.Development => webApplication.DevelopmentConfiguration(),
            _ => throw new NotImplementedException(),
        };
    }

    internal static IApplicationBuilder DevelopmentConfiguration(this WebApplication webApplication)
    {
        webApplication.UseExceptionHandler(
            new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandlingPath = "/Development"
            }
        );
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();

        return webApplication;
    }

    internal static IApplicationBuilder ProductionConfiguration(this WebApplication webApplication)
    {
        webApplication.UseExceptionHandler(
            new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandlingPath = "/Production"
            }
        );

        return webApplication;
    }
}
