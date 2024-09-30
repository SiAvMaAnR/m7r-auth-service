﻿using Auth.Domain.Common;
using Auth.Domain.Exceptions;
using Auth.Domain.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.AppSettings;

public class AppSettings(
    IOptions<CommonSettings> commonSettings,
    IOptions<FilePathSettings> filePathSettings,
    IOptions<RoutePathSettings> routePathSettings,
    IOptions<ClientSettings> clientSettings,
    IOptions<AuthSettings> authSettings,
    IOptions<RMQSettings> rmqSettings
) : IAppSettings
{
    public CommonSettings Common { get; } = commonSettings.Value;
    public FilePathSettings FilePath { get; } = filePathSettings.Value;
    public RoutePathSettings RoutePath { get; } = routePathSettings.Value;
    public ClientSettings Client { get; } = clientSettings.Value;
    public AuthSettings Auth { get; } = authSettings.Value;
    public RMQSettings RMQ { get; } = rmqSettings.Value;

    public static TSection GetSection<TSection>(IConfiguration configuration)
        where TSection : ISettings
    {
        TSection? section = configuration.GetSection(TSection.Path).Get<TSection>();

        if (section == null)
            throw new IncorrectDataException($"Incorrect config ({TSection.Path})", true);

        return section;
    }
}
