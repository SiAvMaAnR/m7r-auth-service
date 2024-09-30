namespace Auth.Domain.Exceptions.Common;

public class ExceptionArgs
{
    public ApiStatusCode ApiStatusCode { get; set; }
    public BusinessStatusCode BusinessStatusCode { get; set; }
    public required string ClientMessage { get; set; }
    public required string SystemMessage { get; set; }
}
