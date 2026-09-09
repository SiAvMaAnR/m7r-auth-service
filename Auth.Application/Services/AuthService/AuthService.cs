using System.Text.Json;
using Auth.Application.Common;
using Auth.Application.Services.AuthService.Models;
using Auth.Application.Services.Common;
using Auth.Domain.Common;
using Auth.Domain.Entities.RefreshTokens;
using Auth.Domain.Exceptions;
using Auth.Domain.Security;
using Auth.Domain.Services;
using Auth.Domain.Shared.Models;
using Auth.Infrastructure.Services.AccountsService;
using Auth.Infrastructure.Services.NotificationsService;
using Auth.Infrastructure.Services.NotificationsService.Models;
using Microsoft.AspNetCore.DataProtection;

namespace Auth.Application.Services.AuthService;

public class AuthService : BaseService, IAuthService
{
    private readonly IDataProtectionProvider _protection;
    private readonly AuthBS _authBS;
    private readonly IAccountsIS _accountsIS;
    private readonly INotificationsIS _notificationsIS;

    public AuthService(
        IUnitOfWork unitOfWork,
        IAppSettings appSettings,
        IDataProtectionProvider protection,
        AuthBS authBS,
        IAccountsIS accountsIS,
        INotificationsIS notificationsIS
    )
        : base(unitOfWork, appSettings)
    {
        _protection = protection;
        _authBS = authBS;
        _accountsIS = accountsIS;
        _notificationsIS = notificationsIS;
    }

    public async Task<AuthServiceLoginResponse> LoginAsync(AuthServiceLoginRequest request)
    {
        Account account =
            await _accountsIS.GetByEmailAsync(request.Email)
            ?? throw new InvalidCredentialsException("Invalid email or password");

        bool isVerify = PasswordHasher.Verify(
            request.Password,
            new Password() { Hash = account.PasswordHash, Salt = account.PasswordSalt }
        );

        if (!isVerify)
            throw new InvalidCredentialsException("Invalid email or password");

        if (account.IsBanned == true)
            throw new OperationNotAllowedException("Account was banned");

        string refreshToken = AuthOptions.CreateRefreshToken();
        string accessToken = AuthOptions.CreateAccessToken(account, _appSettings);

        RefreshToken newRefreshToken = await _authBS.AddRefreshTokenAsync(account, refreshToken);

        return new AuthServiceLoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExp = newRefreshToken.ExpiryTime
        };
    }

    public async Task<AuthServiceRefreshTokenResponse> RefreshTokenAsync(
        AuthServiceRefreshTokenRequest request
    )
    {
        RefreshToken refreshToken =
            await _authBS.GetRefreshTokenAsync(request.RefreshToken)
            ?? throw new InvalidCredentialsException("Invalid refresh token");

        if (refreshToken.ExpiryTime < DateTime.UtcNow)
            throw new OperationNotAllowedException("Expired refresh token");

        Account account =
            await _accountsIS.GetByIdAsync(refreshToken.AccountId)
            ?? throw new InvalidCredentialsException("Invalid refresh token");

        if (account.IsBanned == true)
            throw new OperationNotAllowedException("Account was banned");

        string newRefreshToken = AuthOptions.CreateRefreshToken();
        string accessToken = AuthOptions.CreateAccessToken(account, _appSettings);

        RefreshToken addedRefreshToken = await _authBS.RotateRefreshTokenAsync(
            refreshToken,
            account,
            newRefreshToken
        );

        return new AuthServiceRefreshTokenResponse()
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExp = addedRefreshToken.ExpiryTime
        };
    }

    public async Task<AuthServiceResetTokenResponse> ResetTokenAsync(
        AuthServiceResetTokenRequest request
    )
    {
        Account? account = await _accountsIS.GetByEmailAsync(request.Email);

        if (account is null)
            return new AuthServiceResetTokenResponse() { IsSuccess = true };

        string baseUrl = _appSettings.Client.BaseUrl;

        string path = _appSettings.RoutePath.ResetToken;

        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);

        string resetTokenJson = JsonSerializer.Serialize(
            new ResetToken()
            {
                Id = account.Id,
                Email = request.Email,
                ExpirationDate = DateTime.UtcNow.AddHours(1),
            }
        );

        string resetToken = protector.Protect(resetTokenJson);

        string resetPasswordLink = $"{baseUrl}/{path}?token={resetToken}";

        await _notificationsIS.SendEmailAsync(
            new NotificationsIServiceSendEmailRequest()
            {
                Template = EmailTemplate.ResetPassword,
                Recipient = account.Email,
                Data = new { recipientName = account.Login, resetPasswordLink }
            }
        );

        return new AuthServiceResetTokenResponse() { IsSuccess = true };
    }

    public async Task<AuthServiceResetPasswordResponse> ResetPasswordAsync(
        AuthServiceResetPasswordRequest request
    )
    {
        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);

        string resetTokenJson = protector.Unprotect(request.ResetToken);

        ResetToken resetToken =
            JsonSerializer.Deserialize<ResetToken>(resetTokenJson)
            ?? throw new IncorrectDataException("Incorrect reset token");

        if (resetToken.ExpirationDate < DateTime.UtcNow)
            throw new OperationNotAllowedException("Reset token has expired");

        await _accountsIS.UpdatePasswordAsync(resetToken.Id, request.Password);

        await _authBS.DeleteRefreshTokensByAccountAsync(resetToken.Id);

        return new AuthServiceResetPasswordResponse() { IsSuccess = true };
    }

    public async Task<AuthServiceRevokeTokenResponse> RevokeTokenAsync(
        AuthServiceRevokeTokenRequest request
    )
    {
        RefreshToken refreshToken =
            await _authBS.GetRefreshTokenAsync(request.RefreshToken)
            ?? throw new InvalidCredentialsException("Invalid refresh token");

        await _authBS.DeleteRefreshTokenAsync(refreshToken);

        return new AuthServiceRevokeTokenResponse() { IsSuccess = true };
    }
}
