namespace Auth.WebApi.RMQListeners;

public partial class AuthRMQService
{
    public class LoginData
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
