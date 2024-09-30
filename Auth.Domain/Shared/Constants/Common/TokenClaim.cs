namespace Auth.Domain.Shared.Constants.Common;

public static class TokenClaim
{
    public const string Audience = "audience";
    public const string Issuer = "issuer";
    public const string AccessTokenLifeTime = "accessTokenLifeTime";
    public const string RefreshTokenLifeTime = "refreshTokenLifeTime";
    public const string SecretKey = "secretKey";
}
