using Auth.Domain.Common;
using Auth.Infrastructure.AppSettings;
using Auth.Infrastructure.RabbitMQ;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddSingletonDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAppSettings, AppSettings>();
        serviceCollection.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

        return serviceCollection;
    }
}
