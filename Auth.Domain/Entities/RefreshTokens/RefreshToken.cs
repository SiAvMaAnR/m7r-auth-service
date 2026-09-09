using System.ComponentModel.DataAnnotations.Schema;
using Auth.Domain.Security;

namespace Auth.Domain.Entities.RefreshTokens;

[Table("RefreshTokens")]
public partial class RefreshToken : BaseEntity
{
    public RefreshToken(string token, DateTime expiryTime, int accountId)
    {
        Token = token;
        ExpiryTime = expiryTime;
        AccountId = accountId;
    }

    public string Token { get; private set; }
    public DateTime ExpiryTime { get; private set; }
    public int AccountId { get; private set; }

    public static RefreshToken Create(int accountId, string rawToken, double lifeTimeMinutes)
    {
        string tokenHash = RefreshTokenHasher.Hash(rawToken);
        DateTime expiryTime = DateTime.UtcNow.AddMinutes(lifeTimeMinutes);

        return new RefreshToken(tokenHash, expiryTime, accountId);
    }
}
