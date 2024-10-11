using System.Text;
using System.Text.Json;
using Auth.Domain.Common;
using RabbitMQ.Client;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQBase
{
    protected readonly IConnection _connection;
    protected readonly IModel _channel;
    protected readonly IAppSettings _appSettings;

    public RabbitMQBase(IAppSettings appSettings)
    {
        _appSettings = appSettings;
        _connection = CreateConnection(appSettings);
        _channel = _connection.CreateModel();
    }

    protected void CreateQueue(string queue)
    {
        _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    protected static IConnection CreateConnection(IAppSettings appSettings)
    {
        var factory = new ConnectionFactory()
        {
            HostName = appSettings.RMQ.HostName,
            UserName = appSettings.RMQ.UserName,
            Password = appSettings.RMQ.Password
        };

        return factory.CreateConnection();
    }

    protected static byte[] MessageAdapter(object? message, string? pattern = null)
    {
        string adaptedMessage = JsonSerializer.Serialize(new { pattern, data = message });

        return Encoding.UTF8.GetBytes(adaptedMessage);
    }

    protected static TResponse? Deserialize<TResponse>(string content)
    {
        try
        {
            return JsonSerializer.Deserialize<TResponse>(content);
        }
        catch (Exception)
        {
            throw new JsonException($"Failed deserialization. JSON: {content}");
        }
    }
}
