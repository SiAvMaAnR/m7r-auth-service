using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

namespace Auth.Infrastructure.RabbitMQ;

public static class RMQ
{
    public static class Queue
    {
        public const string Accounts = "accounts-queue";
        public const string Notifications = "notifications-queue";
        public const string Auth = "auth-queue";
    }

    public static class AuthQueuePattern
    {
        public const string Login = "login";
    }

    public static class NotificationsQueuePattern
    {
        public const string SendEmail = "send";
    }

    public static class AccountsQueuePattern
    {
        public const string UpdatePassword = "update-password";
        public const string GetById = "get-by-id";
        public const string GetByEmail = "get-by-email";
    }
}

public class RMQError
{
    public string? ClientMessage { get; set; }
    public required string Message { get; set; }
}

public class RMQResponse<TData>
{
    [JsonPropertyName("pattern")]
    public required string Pattern { get; set; }

    [JsonPropertyName("data")]
    public required TData? Data { get; set; }

    [JsonPropertyName("error")]
    public required RMQError? Error { get; set; }
}

public class DeliverEventData
{
    public required RMQResponse<JsonElement> DeserializedResponse { get; set; }
    public required IBasicProperties BasicProperties { get; set; }
    public required string ReplyQueue { get; set; }
    public required string CorrelationId { get; set; }
    public required JsonSerializerOptions SerializerOptions { get; set; }
}
