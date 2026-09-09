using Auth.Domain.Specification;

namespace Auth.Domain.Entities.RefreshTokens;

public class RefreshTokenByTokenSpec : Specification<RefreshToken>
{
    public RefreshTokenByTokenSpec(string refreshToken)
        : base((token) => token.Token == refreshToken) { }
}

public class RefreshTokensByAccountSpec : Specification<RefreshToken>
{
    public RefreshTokensByAccountSpec(int accountId)
        : base((token) => token.AccountId == accountId) { }
}
