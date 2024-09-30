using Auth.Domain.Shared.Settings;

namespace Auth.Domain.Common;

public interface IAppSettings
{
    CommonSettings Common { get; }
    FilePathSettings FilePath { get; }
    RoutePathSettings RoutePath { get; }
    ClientSettings Client { get; }
    AuthSettings Auth { get; }
    RMQSettings RMQ { get; }
}
