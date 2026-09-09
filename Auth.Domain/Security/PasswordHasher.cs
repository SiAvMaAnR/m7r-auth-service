using System.Security.Cryptography;
using System.Text;
using Auth.Domain.Exceptions;
using Auth.Domain.Shared.Constants.Common;
using Auth.Domain.Shared.Models;

namespace Auth.Domain.Security;

public static class PasswordHasher
{
    public static Password Create(string password)
    {
        try
        {
            byte[] salt = RandomNumberGenerator.GetBytes(PasswordHash.SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                PasswordHash.Iterations,
                PasswordHash.Algorithm,
                PasswordHash.Size
            );

            return new Password() { Salt = salt, Hash = hash };
        }
        catch (Exception)
        {
            throw new FailedToCreatePasswordException();
        }
    }

    public static bool Verify(string password, Password targetPassword)
    {
        try
        {
            byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                targetPassword.Salt,
                PasswordHash.Iterations,
                PasswordHash.Algorithm,
                PasswordHash.Size
            );

            return CryptographicOperations.FixedTimeEquals(computedHash, targetPassword.Hash);
        }
        catch (Exception)
        {
            throw new FailedToVerifyPasswordException();
        }
    }
}
