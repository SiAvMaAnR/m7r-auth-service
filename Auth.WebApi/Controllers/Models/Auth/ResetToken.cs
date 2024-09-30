using System.ComponentModel.DataAnnotations;
using Auth.Domain.Shared.Constants.Validation;

namespace Auth.WebApi.Controllers.Models.Auth;

public class AuthControllerResetTokenRequest
{
    [EmailAddress, MaxLength(MaxLength.Email)]
    public required string Email { get; set; }
}
