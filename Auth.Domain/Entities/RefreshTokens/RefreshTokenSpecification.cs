using Auth.Domain.Entities.RefreshTokens;
using Auth.Domain.Specification;

namespace Auth.Domain.Entities.Accounts;

public class RefreshTokenByTokenSpec : Specification<RefreshToken>
{
    public RefreshTokenByTokenSpec(string? refreshToken)
        : base((token) => token.Token == refreshToken) { }
}
