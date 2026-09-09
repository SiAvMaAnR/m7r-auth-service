using System.Security.Cryptography;
using System.Text;

namespace Auth.Domain.Security;

public static class RefreshTokenHasher
{
    public static string Hash(string refreshToken)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(hash);
    }
}
