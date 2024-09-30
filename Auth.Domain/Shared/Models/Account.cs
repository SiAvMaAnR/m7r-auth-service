using Auth.Domain.Shared.Constants.Common;

namespace Auth.Domain.Shared.Models;

public class Account
{
    public required int Id { get; set; }
    public required string Login { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; } = AccountRole.Public;
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required bool IsBanned { get; set; } = false;
}
