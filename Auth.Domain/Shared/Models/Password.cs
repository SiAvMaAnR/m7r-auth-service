namespace Auth.Domain.Shared.Models;

public class Password
{
    public required byte[] Salt { get; set; }
    public required byte[] Hash { get; set; }
}
