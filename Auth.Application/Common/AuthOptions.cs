using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Common;
using Auth.Domain.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Common;

public static class AuthOptions
{
    public static string CreateAccessToken(Account account, IAppSettings appSettings)
    {
        byte[] hashSecretKey = SHA512.HashData(
            Encoding.UTF8.GetBytes(appSettings.Common.SecretKey)
        );
        var key = new SymmetricSecurityKey(hashSecretKey);

        DateTime expires = DateTime.UtcNow.AddMinutes(
            double.Parse(appSettings.Auth.AccessTokenLifeTime)
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            audience: appSettings.Auth.Audience,
            issuer: appSettings.Auth.Issuer,
            claims: GetClaims(account),
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string CreateRefreshToken()
    {
        byte[] randomNumber = new byte[256];
        RandomNumberGenerator.Create().GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static List<Claim> GetClaims(Account account)
    {
        return
        [
            new(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new(ClaimTypes.Name, account.Login),
            new(ClaimTypes.Email, account.Email),
            new(ClaimTypes.Role, account.Role)
        ];
    }
}
