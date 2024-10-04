using System.Text.Json.Serialization;

namespace Auth.Infrastructure.RabbitMQ;

public static class RMQ
{
    public static class Queue
    {
        public const string Accounts = "accounts-queue";
        public const string Notifications = "notifications-queue";
    }

    public static class AccountsQueuePattern
    {
        public const string UpdatePassword = "update-password";
        public const string GetById = "get-by-id";
        public const string GetByEmail = "get-by-email";
    }

    public static class NotificationsQueuePattern
    {
        public const string SendEmail = "send";
    }
}

public class RMQResponse<TData>
{
    [JsonPropertyName("pattern")]
    public required string Pattern { get; set; }

    [JsonPropertyName("data")]
    public required TData Data { get; set; }
}
