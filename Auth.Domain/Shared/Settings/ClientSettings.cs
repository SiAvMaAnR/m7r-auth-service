namespace Auth.Domain.Shared.Settings;

public class ClientSettings : ISettings
{
    public static string Path => "Client";
    public required string BaseUrl { get; set; }
}
