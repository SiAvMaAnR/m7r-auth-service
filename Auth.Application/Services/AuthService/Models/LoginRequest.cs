namespace Auth.Application.Services.AuthService.Models;

public class AuthServiceLoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
