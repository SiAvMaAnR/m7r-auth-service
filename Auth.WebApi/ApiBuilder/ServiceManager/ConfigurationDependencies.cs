using Auth.Domain.Shared.Settings;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddConfigurationDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config
    )
    {
        serviceCollection.Configure<CommonSettings>(config.GetSection(CommonSettings.Path));
        serviceCollection.Configure<FilePathSettings>(config.GetSection(FilePathSettings.Path));
        serviceCollection.Configure<AuthSettings>(config.GetSection(AuthSettings.Path));
        serviceCollection.Configure<ClientSettings>(config.GetSection(ClientSettings.Path));
        serviceCollection.Configure<RoutePathSettings>(config.GetSection(RoutePathSettings.Path));
        serviceCollection.Configure<FilePathSettings>(config.GetSection(FilePathSettings.Path));
        serviceCollection.Configure<RMQSettings>(config.GetSection(RMQSettings.Path));

        return serviceCollection;
    }
}
