using Auth.Persistence.DBContext;
using Auth.WebApi.ApiBuilder.Configurations;
using Auth.WebApi.Common;
using Microsoft.EntityFrameworkCore;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddCommonDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config
    )
    {
        string? connection = AppEnvironment.GetDBConnectionString(config);

        serviceCollection.AddOptions();
        serviceCollection.AddDbContext<EFContext>(options => options.UseNpgsql(connection));
        serviceCollection.AddHealthChecks();
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddLogging();
        serviceCollection.AddCors(options => options.CorsConfig(config));
        serviceCollection.AddRateLimiter(options => options.RateLimiterConfig());
        serviceCollection.AddSwaggerGen(options => options.Config());
        serviceCollection.AddDataProtection();
        serviceCollection.AddSignalR();
        serviceCollection.AddHttpClient();

        return serviceCollection;
    }
}
