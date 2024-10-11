using Auth.Domain.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQConsumer : RabbitMQBase, IRabbitMQConsumer
{
    private readonly EventingBasicConsumer _consumer;

    public RabbitMQConsumer(IAppSettings appSettings)
        : base(appSettings)
    {
        _consumer = new EventingBasicConsumer(_channel);
    }

    public void AddListener(string queueName, EventHandler<BasicDeliverEventArgs> handler)
    {
        CreateQueue(queueName);

        _consumer.Received += handler;

        _channel.BasicConsume(
            queue: queueName,
            autoAck: true,
            consumer: _consumer,
            consumerTag: Guid.NewGuid().ToString()
        );
    }

    public void RemoveListener(EventHandler<BasicDeliverEventArgs> handler)
    {
        _consumer.Received -= handler;
    }
}
