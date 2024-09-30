using Auth.Domain.Entities.RefreshTokens;

namespace Auth.Domain.Common;

public partial interface IUnitOfWork
{
    IRefreshTokenRepository RefreshToken { get; }
}
