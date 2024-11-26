using Auth.WebApi.RMQListeners;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddHostedDependencies(
        this IServiceCollection serviceCollection
    )
    {
        serviceCollection.AddHostedService<AuthRMQService>();

        return serviceCollection;
    }
}
