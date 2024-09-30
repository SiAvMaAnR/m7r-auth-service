namespace Auth.Domain.Shared.Settings;

public class CommonSettings : ISettings
{
    public static string Path => "Common";

    public string SecretKey { get; set; } = null!;
}
