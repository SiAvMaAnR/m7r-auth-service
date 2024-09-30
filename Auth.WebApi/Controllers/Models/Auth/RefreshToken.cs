using System.ComponentModel.DataAnnotations;
using Auth.Domain.Shared.Constants.Validation;

namespace Auth.WebApi.Controllers.Models.Auth;

public class AuthControllerRefreshTokenRequest
{
    [MaxLength(MaxLength.RefreshToken)]
    public required string RefreshToken { get; set; }
}
