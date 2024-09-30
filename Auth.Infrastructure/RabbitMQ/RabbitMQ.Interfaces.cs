namespace Auth.Infrastructure.RabbitMQ;

public interface IRabbitMQProducer
{
    void Send(string queue, string pattern, object message);
    Task<TResponse?> Emit<TResponse>(string queue, string pattern, object message);
}
