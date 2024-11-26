using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Shared.Constants.Common;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Common;

public static class AuthOptions
{
    public static string CreateAccessToken(List<Claim> claims, Dictionary<string, string> tokenParams)
    {
        byte[] hashSecretKey = SHA512.HashData(Encoding.UTF8.GetBytes(tokenParams[TokenClaim.SecretKey]));
        var key = new SymmetricSecurityKey(hashSecretKey);

        DateTime expires = DateTime.Now.AddMinutes(double.Parse(tokenParams[TokenClaim.AccessTokenLifeTime]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            audience: tokenParams[TokenClaim.Audience],
            issuer: tokenParams[TokenClaim.Issuer],
            claims: claims,
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
}
