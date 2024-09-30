using Auth.Domain.Common;
using Auth.Infrastructure.RabbitMQ;

namespace Auth.Infrastructure.Services.Common;

public abstract class BaseIService(IAppSettings appSettings, IRabbitMQProducer rabbitMQProducer)
{
    protected readonly IAppSettings _appSettings = appSettings;
    protected readonly IRabbitMQProducer _rabbitMQProducer = rabbitMQProducer;
}
