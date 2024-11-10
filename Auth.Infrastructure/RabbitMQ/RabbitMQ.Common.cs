using System.Text;
using System.Text.Json;
using Auth.Domain.Common;
using Auth.Domain.Exceptions;
using Auth.Domain.Exceptions.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQBase
{
    protected readonly IConnection _connection;
    protected readonly IModel _channel;
    protected readonly IAppSettings _appSettings;
    public static readonly JsonSerializerOptions JsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public RabbitMQBase(IAppSettings appSettings)
    {
        _appSettings = appSettings;
        _connection = CreateConnection(appSettings);
        _channel = _connection.CreateModel();
    }

    public static DeliverEventData GetDeliverEventData(BasicDeliverEventArgs args)
    {
        byte[] body = args.Body.ToArray();
        string bodyJson = Encoding.UTF8.GetString(body);

        RMQResponse<JsonElement> deserializedResponse =
            JsonSerializer.Deserialize<RMQResponse<JsonElement>>(bodyJson, JsonSerializerOptions)
            ?? throw new IncorrectDataException("Failed to deserialize json");

        string replyQueue = args.BasicProperties.ReplyTo;
        string correlationId = args.BasicProperties.CorrelationId;

        return new DeliverEventData()
        {
            BasicProperties = args.BasicProperties,
            DeserializedResponse = deserializedResponse,
            ReplyQueue = replyQueue,
            CorrelationId = correlationId,
            SerializerOptions = JsonSerializerOptions
        };
    }

    protected void CreateQueue(string queue)
    {
        _channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
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
        bool isException = message is Exception;

        string adaptedMessage = JsonSerializer.Serialize(
            new
            {
                pattern,
                data = isException ? null : message,
                error = isException
                    ? new
                    {
                        (message as Exception)?.Message,
                        (message as BusinessException)?.ClientMessage,
                    }
                    : null
            },
            JsonSerializerOptions
        );

        return Encoding.UTF8.GetBytes(adaptedMessage);
    }

    protected static TResponse? Deserialize<TResponse>(string content)
    {
        try
        {
            return JsonSerializer.Deserialize<TResponse>(content, JsonSerializerOptions);
        }
        catch (Exception)
        {
            throw new JsonException($"Failed deserialization. JSON: {content}");
        }
    }
}
