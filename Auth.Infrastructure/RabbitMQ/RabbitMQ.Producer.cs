using System.Text;
using Auth.Domain.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IAppSettings _appSettings;

    public RabbitMQProducer(IAppSettings appSettings)
    {
        _appSettings = appSettings;
        _connection = RabbitMQ.CreateConnection(appSettings);
        _channel = _connection.CreateModel();
    }

    public void Send(string queue, string pattern, object message)
    {
        byte[] body = RabbitMQ.MessageAdapter(message, pattern);

        _channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            basicProperties: null,
            body: body
        );
    }

    public async Task<TResponse?> Emit<TResponse>(string queue, string pattern, object message)
    {
        byte[] body = RabbitMQ.MessageAdapter(message, pattern);

        _channel.QueueDeclare(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var tcs = new TaskCompletionSource<string>();
        var consumer = new EventingBasicConsumer(_channel);
        string correlationId = Guid.NewGuid().ToString();
        string replyQueueName = _channel.QueueDeclare(autoDelete: true).QueueName;

        string consumerTag = _channel.BasicConsume(
            queue: replyQueueName,
            autoAck: true,
            consumer: consumer
        );

        consumer.Received += (model, eventArgs) =>
        {
            if (eventArgs.BasicProperties.CorrelationId == correlationId)
            {
                byte[] body = eventArgs.Body.ToArray();
                string response = Encoding.UTF8.GetString(body);
                tcs.SetResult(response);
                _channel.BasicCancel(consumerTag);
            }
        };

        IBasicProperties props = _channel.CreateBasicProperties();
        props.CorrelationId = correlationId;
        props.ReplyTo = replyQueueName;

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            basicProperties: props,
            body: body
        );

        var delayTask = Task.Delay(_appSettings.RMQ.Timeout);
        Task completedTask = await Task.WhenAny(tcs.Task, delayTask);

        if (completedTask == delayTask)
            throw new TimeoutException("The request timed out");

        string result = await tcs.Task;

        return RabbitMQ.Deserialize<TResponse>(result);
    }
}
