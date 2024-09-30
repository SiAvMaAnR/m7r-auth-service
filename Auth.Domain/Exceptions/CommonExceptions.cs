using Auth.Domain.Exceptions.Common;

namespace Auth.Domain.Exceptions;

public class NotExistsException : BusinessException
{
    public NotExistsException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.BadRequest,
                BusinessStatusCode = BusinessStatusCode.CommonE001,
                SystemMessage = $"Not exists: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}

public class AlreadyExistsException : BusinessException
{
    public AlreadyExistsException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.BadRequest,
                BusinessStatusCode = BusinessStatusCode.CommonE002,
                SystemMessage = $"Already exists: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}

public class IncorrectDataException : BusinessException
{
    public IncorrectDataException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.BadRequest,
                BusinessStatusCode = BusinessStatusCode.CommonE003,
                SystemMessage = $"Incorrect data: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}

public class OperationNotAllowedException : BusinessException
{
    public OperationNotAllowedException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.Forbidden,
                BusinessStatusCode = BusinessStatusCode.CommonE004,
                SystemMessage = $"Operation is not allowed: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}

public class SomethingWentWrongException : BusinessException
{
    public SomethingWentWrongException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.InternalServer,
                BusinessStatusCode = BusinessStatusCode.CommonE005,
                SystemMessage = $"Something went wrong: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}

public class IncorrectConfigException : BusinessException
{
    public IncorrectConfigException(string reason = "none", bool isDefaultClientMessage = false)
        : base(
            new ExceptionArgs()
            {
                ApiStatusCode = ApiStatusCode.InternalServer,
                BusinessStatusCode = BusinessStatusCode.CommonE006,
                SystemMessage = $"Incorrect config: {reason}",
                ClientMessage = isDefaultClientMessage ? DefaultClientMessage : reason
            }
        )
    { }
}
