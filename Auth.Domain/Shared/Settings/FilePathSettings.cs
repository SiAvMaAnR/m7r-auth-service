namespace Auth.Domain.Shared.Settings;

public class FilePathSettings : ISettings
{
    public static string Path => "FilePath";

    public string Logger { get; set; } = null!;
}
