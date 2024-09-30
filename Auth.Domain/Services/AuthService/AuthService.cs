using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Common;
using Auth.Domain.Entities.Accounts;
using Auth.Domain.Entities.RefreshTokens;
using Auth.Domain.Exceptions;
using Auth.Domain.Shared.Constants.Common;
using Auth.Domain.Shared.Models;

namespace Auth.Domain.Services;

public class AuthBS : DomainService
{
    public AuthBS(IAppSettings appSettings, IUnitOfWork unitOfWork)
        : base(appSettings, unitOfWork) { }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string? refreshToken)
    {
        return await _unitOfWork.RefreshToken.GetAsync(new RefreshTokenByTokenSpec(refreshToken));
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(Account account, string refreshToken)
    {
        double refreshTokenLifeTime = double.Parse(_appSettings.Auth.RefreshTokenLifeTime);

        DateTime expiryTime = DateTime.Now.AddMinutes(refreshTokenLifeTime);

        var newRefreshToken = new RefreshToken(refreshToken, expiryTime, account.Id);

        await _unitOfWork.RefreshToken.AddAsync(newRefreshToken);
        await _unitOfWork.SaveChangesAsync();

        return newRefreshToken;
    }

    public async Task DeleteRefreshTokenAsync(RefreshToken refreshToken)
    {
        _unitOfWork.RefreshToken.Delete(refreshToken);
        await _unitOfWork.SaveChangesAsync();
    }

    public Dictionary<string, string> GetTokenParams()
    {
        var tokenParams = new Dictionary<string, string>()
        {
            { TokenClaim.SecretKey, _appSettings.Common.SecretKey },
            { TokenClaim.Audience, _appSettings.Auth.Audience },
            { TokenClaim.Issuer, _appSettings.Auth.Issuer },
            { TokenClaim.AccessTokenLifeTime, _appSettings.Auth.AccessTokenLifeTime },
        };

        return tokenParams;
    }

    public static List<Claim> GetClaims(Account account)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new(ClaimTypes.Name, account.Login),
            new(ClaimTypes.Email, account.Email),
            new(ClaimTypes.Role, account.Role)
        ];

        return claims;
    }

    public static Password CreatePasswordHash(string password)
    {
        try
        {
            var hmac = new HMACSHA512();

            return new Password()
            {
                Salt = hmac.Key,
                Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            };
        }
        catch (Exception)
        {
            throw new FailedToCreatePasswordException();
        }
    }

    public static bool VerifyPasswordHash(string password, Password targetPassword)
    {
        try
        {
            var hmac = new HMACSHA512(targetPassword.Salt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(targetPassword.Hash);
        }
        catch (Exception)
        {
            throw new FailedToVerifyPasswordException();
        }
    }
}
