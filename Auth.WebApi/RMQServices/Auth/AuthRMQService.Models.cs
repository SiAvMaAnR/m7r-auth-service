using System.Text.Json.Serialization;

namespace Auth.WebApi.RMQServices;

public partial class AuthRMQService
{
    public class LoginData
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
