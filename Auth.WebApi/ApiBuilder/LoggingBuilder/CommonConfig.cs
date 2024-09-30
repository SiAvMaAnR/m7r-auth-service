using Auth.Domain.Shared.Settings;
using NReco.Logging.File;

namespace Auth.WebApi.ApiBuilder.LoggingBuilder;

public static partial class LoggingBuilderExtension
{
    public static ILoggingBuilder AddCommonConfiguration(
        this ILoggingBuilder loggingBuilder,
        IConfiguration configuration
    )
    {
        var filePathSettings = new FilePathSettings();

        configuration.GetSection(FilePathSettings.Path).Bind(filePathSettings);

        loggingBuilder.ClearProviders();
        loggingBuilder.AddConsole();
        loggingBuilder.AddFile(
            $"{filePathSettings.Logger}/information.log",
            config =>
            {
                config.Append = true;
                config.MinLevel = LogLevel.Information;
            }
        );
        loggingBuilder.AddFile(
            $"{filePathSettings.Logger}/error.log",
            config =>
            {
                config.Append = true;
                config.MinLevel = LogLevel.Error;
            }
        );
        loggingBuilder.AddFile(
            $"{filePathSettings.Logger}/trace.log",
            config =>
            {
                config.Append = true;
                config.MinLevel = LogLevel.Trace;
            }
        );

        return loggingBuilder;
    }
}
