using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public interface IRabbitMQProducer
{
    void Emit(
        string queue,
        string pattern,
        object? message,
        string? correlationId = null,
        string? replyQueue = null
    );
    Task<TResponse?> Send<TResponse>(string queue, string pattern, object message);
}

public interface IRabbitMQConsumer
{
    void AddListener(string queueName, Func<object?, DeliverEventData, Task> handler);
    void RemoveListener(EventHandler<BasicDeliverEventArgs> handler);
}
