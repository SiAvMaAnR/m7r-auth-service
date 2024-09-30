namespace Auth.Domain.Shared.Models;

public class ResetToken
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public DateTime ExpirationDate { get; set; }
}
