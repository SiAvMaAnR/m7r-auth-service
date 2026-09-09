using Auth.Domain.Common;
using Auth.Domain.Entities.RefreshTokens;
using Auth.Domain.Security;
using Auth.Domain.Shared.Models;

namespace Auth.Domain.Services;

public class AuthBS : DomainService
{
    public AuthBS(IAppSettings appSettings, IUnitOfWork unitOfWork)
        : base(appSettings, unitOfWork) { }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        string tokenHash = RefreshTokenHasher.Hash(refreshToken);

        return await _unitOfWork.RefreshToken.GetAsync(new RefreshTokenByTokenSpec(tokenHash));
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(Account account, string refreshToken)
    {
        double lifeTime = double.Parse(_appSettings.Auth.RefreshTokenLifeTime);
        var newRefreshToken = RefreshToken.Create(account.Id, refreshToken, lifeTime);

        await _unitOfWork.RefreshToken.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return newRefreshToken;
    }

    public async Task<RefreshToken> RotateRefreshTokenAsync(
        RefreshToken oldRefreshToken,
        Account account,
        string newRefreshToken
    )
    {
        _unitOfWork.RefreshToken.Delete(oldRefreshToken);

        double lifeTime = double.Parse(_appSettings.Auth.RefreshTokenLifeTime);
        var addedRefreshToken = RefreshToken.Create(account.Id, newRefreshToken, lifeTime);
        await _unitOfWork.RefreshToken.AddAsync(addedRefreshToken);

        await _unitOfWork.SaveChangesAsync();

        return addedRefreshToken;
    }

    public async Task DeleteRefreshTokenAsync(RefreshToken refreshToken)
    {
        _unitOfWork.RefreshToken.Delete(refreshToken);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteRefreshTokensByAccountAsync(int accountId)
    {
        await _unitOfWork.RefreshToken.DeleteAllAsync(new RefreshTokensByAccountSpec(accountId));
    }
}
