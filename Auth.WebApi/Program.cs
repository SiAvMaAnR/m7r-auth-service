using Auth.WebApi.ApiBuilder.ApplicationBuilder;
using Auth.WebApi.ApiBuilder.LoggingBuilder;
using Auth.WebApi.ApiBuilder.ServiceManager;
using Auth.WebApi.Common;

AppEnvironment.LoadEnvironments();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddConfigurationDependencies(config);
builder.Services.AddCommonDependencies(config);
builder.Services.AddTransientDependencies();
builder.Services.AddScopedDependencies();
builder.Services.AddSingletonDependencies();
builder.Services.AddHostedDependencies();

builder.Logging.AddCommonConfiguration(config);

WebApplication application = builder.Build();

application.AddEnvironmentConfiguration();
application.CommonConfiguration();

application.Run();
