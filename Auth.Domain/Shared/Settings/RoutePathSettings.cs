namespace Auth.Domain.Shared.Settings;

public class RoutePathSettings : ISettings
{
    public static string Path => "RoutePath";

    public string ResetToken { get; set; } = null!;
}
