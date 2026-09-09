using System.ComponentModel.DataAnnotations;
using Auth.Domain.Shared.Constants.Validation;

namespace Auth.WebApi.Controllers.Models.Auth;

public class AuthControllerRevokeTokenRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(MaxLength.RefreshToken)]
    public required string RefreshToken { get; set; }
}
