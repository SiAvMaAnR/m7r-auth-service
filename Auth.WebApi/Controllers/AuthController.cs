using Auth.Application.Services.AuthService;
using Auth.Application.Services.AuthService.Models;
using Auth.WebApi.Controllers.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthControllerLoginRequest request)
    {
        AuthServiceLoginResponse response = await _authService.LoginAsync(
            new AuthServiceLoginRequest() { Email = request.Email, Password = request.Password }
        );

        return Ok(response);
    }

    [HttpPost("reset-token")]
    public async Task<IActionResult> ResetToken([FromBody] AuthControllerResetTokenRequest request)
    {
        AuthServiceResetTokenResponse response = await _authService.ResetTokenAsync(
            new AuthServiceResetTokenRequest() { Email = request.Email }
        );

        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] AuthControllerResetPasswordRequest request
    )
    {
        AuthServiceResetPasswordResponse response = await _authService.ResetPasswordAsync(
            new AuthServiceResetPasswordRequest()
            {
                ResetToken = request.ResetToken,
                Password = request.Password
            }
        );

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] AuthControllerRefreshTokenRequest request
    )
    {
        AuthServiceRefreshTokenResponse response = await _authService.RefreshTokenAsync(
            new AuthServiceRefreshTokenRequest() { RefreshToken = request.RefreshToken }
        );

        return Ok(response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(
        [FromBody] AuthControllerRevokeTokenRequest request
    )
    {
        AuthServiceRevokeTokenResponse response = await _authService.RevokeTokenAsync(
            new AuthServiceRevokeTokenRequest() { RefreshToken = request.RefreshToken }
        );

        return Ok(response);
    }
}
