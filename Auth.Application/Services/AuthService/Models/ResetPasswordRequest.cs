namespace Auth.Application.Services.AuthService.Models;

public class AuthServiceResetPasswordRequest
{
    public required string ResetToken { get; set; }
    public required string Password { get; set; }
}
