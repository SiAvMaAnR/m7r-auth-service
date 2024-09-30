using Auth.Domain.Common;
using Auth.Infrastructure.AppSettings;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddSingletonDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IAppSettings, AppSettings>();

        return serviceCollection;
    }
}
