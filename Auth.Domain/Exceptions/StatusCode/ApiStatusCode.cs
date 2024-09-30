namespace Auth.Domain.Exceptions;

public enum ApiStatusCode
{
    Ok = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    InternalServer = 500,
}
