using Auth.Domain.Common;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Auth.Infrastructure.RabbitMQ;

public class RabbitMQConsumer : RabbitMQBase, IRabbitMQConsumer
{
    private readonly EventingBasicConsumer _consumer;
    private readonly IRabbitMQProducer _producer;
    private readonly ILogger<RabbitMQConsumer> _logger;

    public RabbitMQConsumer(
        IAppSettings appSettings,
        IRabbitMQProducer producer,
        ILogger<RabbitMQConsumer> logger
    )
        : base(appSettings)
    {
        _producer = producer;
        _consumer = new EventingBasicConsumer(_channel);
        _logger = logger;
    }

    public void AddListener(string queueName, Func<object?, DeliverEventData, Task> handler)
    {
        CreateQueue(queueName);

        _consumer.Received += async (sender, args) =>
        {
            DeliverEventData? deliverEventData = null;

            try
            {
                if (args.RoutingKey == queueName)
                {
                    deliverEventData = GetDeliverEventData(args);

                    await handler(sender, deliverEventData);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "RabbitMQ listener: {Message}", exception.Message);

                string? pattern = deliverEventData?.DeserializedResponse.Pattern;
                string? queue = deliverEventData?.ReplyQueue;

                if (pattern != null && queue != null)
                {
                    _producer.Emit(
                        args.BasicProperties.ReplyTo,
                        pattern,
                        exception,
                        args.BasicProperties.CorrelationId
                    );
                }
            }
        };

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
