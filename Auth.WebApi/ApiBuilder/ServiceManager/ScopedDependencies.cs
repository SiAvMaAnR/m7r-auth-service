using Auth.Application.Services.AuthService;
using Auth.Domain.Common;
using Auth.Domain.Services;
using Auth.Infrastructure.RabbitMQ;
using Auth.Infrastructure.Services.NotificationsService;
using Auth.Persistence.UnitOfWork;

namespace Auth.WebApi.ApiBuilder.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddScopedDependencies(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

        serviceCollection.AddScoped<INotificationsIS, NotificationsIS>();
        serviceCollection.AddScoped<IAccountsIS, AccountsIS>();

        serviceCollection.AddScoped<IAuthService, AuthService>();

        serviceCollection.AddScoped<AuthBS>();

        return serviceCollection;
    }
}
