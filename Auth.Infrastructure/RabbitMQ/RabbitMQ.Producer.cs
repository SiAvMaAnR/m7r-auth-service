using System.Text;
using Auth.Domain.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQProducer : RabbitMQBase, IRabbitMQProducer
{
    public RabbitMQProducer(IAppSettings appSettings)
        : base(appSettings) { }

    public void Emit(
        string queue,
        string pattern,
        object? message,
        string? correlationId = null,
        string? replyQueue = null
    )
    {
        byte[] body = MessageAdapter(message, pattern);

        if (correlationId == null)
        {
            CreateQueue(queue);
        }

        IBasicProperties properties = _channel.CreateBasicProperties();

        properties.CorrelationId = correlationId;
        properties.ReplyTo = replyQueue;

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queue,
            basicProperties: properties,
            body: body
        );
    }

    public async Task<TResponse?> Send<TResponse>(string queue, string pattern, object message)
    {
        byte[] body = MessageAdapter(message, pattern);

        CreateQueue(queue);

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

        return Deserialize<TResponse>(result);
    }
}
