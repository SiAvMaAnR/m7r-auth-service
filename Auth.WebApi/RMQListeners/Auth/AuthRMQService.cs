using System.Text.Json;
using Auth.Application.Services.AuthService;
using Auth.Application.Services.AuthService.Models;
using Auth.Domain.Exceptions;
using Auth.Infrastructure.RabbitMQ;
using RabbitMQ.Client;

namespace Auth.WebApi.RMQListeners;

public partial class AuthRMQService : RMQService
{
    private readonly string _queueName = RMQ.Queue.Auth;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthRMQService(
        IRabbitMQConsumer consumer,
        IRabbitMQProducer producer,
        IServiceScopeFactory serviceScopeFactory
    )
        : base(consumer, producer)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task LoginAsync(
        IBasicProperties basicProperties,
        LoginData args,
        IAuthService authService
    )
    {
        AuthServiceLoginResponse response = await authService.LoginAsync(
            new AuthServiceLoginRequest() { Email = args.Email, Password = args.Password }
        );

        _producer.Emit(
            basicProperties.ReplyTo,
            RMQ.AuthQueuePattern.Login,
            response,
            basicProperties.CorrelationId
        );
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.AddListener(
            _queueName,
            async (_, args) =>
            {
                DeliverEventData deliverEventData = RabbitMQBase.GetDeliverEventData(args);

                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                IAuthService authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                RMQResponse<JsonElement> deserializedResponse =
                    deliverEventData.DeserializedResponse;

                Task task = deserializedResponse.Pattern switch
                {
                    RMQ.AuthQueuePattern.Login
                        => LoginAsync(
                            args.BasicProperties,
                            JsonSerializer.Deserialize<LoginData>(
                                deserializedResponse.Data,
                                deliverEventData.SerializerOptions
                            )!,
                            authService
                        ),
                    _ => throw new OperationNotAllowedException("Message pattern not found")
                };

                await task;
            }
        );

        return Task.CompletedTask;
    }
}
