namespace Auth.Domain.Shared.Constants.Validation;

public static class MaxLength
{
    public const int Email = 100;
    public const int Login = 40;
    public const int Password = 80;
    public const int ResetToken = 1000;
    public const int RefreshToken = 1000;
    public const int Confirmation = 1000;
}
