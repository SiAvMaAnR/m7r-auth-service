using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public interface IRabbitMQProducer
{
    void Send(string queue, string pattern, object? message);
    void SendReply(string queue, string correlationId, string pattern, object? message);
    Task<TResponse?> Emit<TResponse>(string queue, string pattern, object message);
}

public interface IRabbitMQConsumer
{
    void AddListener(string queueName, EventHandler<BasicDeliverEventArgs> handler);
    void RemoveListener(EventHandler<BasicDeliverEventArgs> handler);
}
