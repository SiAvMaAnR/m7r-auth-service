namespace Auth.Application.Services.AuthService.Models;

public class AuthServiceLoginResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExp { get; set; }
}
