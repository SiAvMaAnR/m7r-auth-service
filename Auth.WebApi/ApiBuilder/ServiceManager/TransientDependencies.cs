namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddTransientDependencies(
        this IServiceCollection serviceCollection
    )
    {
        return serviceCollection;
    }
}
