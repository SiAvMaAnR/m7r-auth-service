using System.ComponentModel.DataAnnotations;
using Auth.Domain.Shared.Constants.Validation;

namespace Auth.WebApi.Controllers.Models.Auth;

public class AuthControllerResetPasswordRequest
{
    [MaxLength(MaxLength.ResetToken)]
    public required string ResetToken { get; set; }

    [MinLength(MinLength.Password), MaxLength(MaxLength.Password)]
    public required string Password { get; set; }
}
