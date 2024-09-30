namespace Auth.Domain.Exceptions.Common;

public abstract class BusinessException : Exception, IException
{
    protected const string DefaultClientMessage = "Something went wrong";
    public ApiStatusCode ApiStatusCode { get; }
    public BusinessStatusCode BusinessStatusCode { get; }
    public string ClientMessage { get; }

    public BusinessException(ExceptionArgs exceptionArgs)
        : base(exceptionArgs.SystemMessage)
    {
        ApiStatusCode = exceptionArgs.ApiStatusCode;
        BusinessStatusCode = exceptionArgs.BusinessStatusCode;
        ClientMessage = exceptionArgs.ClientMessage;
    }
}
